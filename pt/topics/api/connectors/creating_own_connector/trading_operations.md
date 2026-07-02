# Operações de Negociação

Ao criar seu próprio adaptador para uma bolsa, implemente os métodos que executam as operações de negociação: registro, substituição e cancelamento de ordens. Esses métodos são chamados quando o adaptador recebe as mensagens correspondentes do núcleo do StockSharp.

## Registro de Ordem

Para registrar uma nova ordem, é implementado o método **RegisterOrderAsync**. Esse método é chamado ao receber a mensagem [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage).

As principais etapas ao registrar uma ordem:

1. Verificação do tipo de ordem e condições adicionais.
2. Conversão dos parâmetros da ordem para um formato compreendido pela bolsa.
3. Envio de uma solicitação para registrar a ordem através da API da bolsa.
4. Processamento da resposta da bolsa e envio da mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondente.

```cs
public override async ValueTask RegisterOrderAsync(OrderRegisterMessage regMsg, CancellationToken cancellationToken)
{
	var condition = (CoinbaseOrderCondition)regMsg.Condition;

	switch (regMsg.OrderType)
	{
		case null:
		case OrderTypes.Limit:
		case OrderTypes.Market:
			break;
		case OrderTypes.Conditional:
		{
			// Handling conditional orders, for example, withdrawal of funds
			if (!condition.IsWithdraw)
				break;

			var withdrawId = await _restClient.Withdraw(regMsg.SecurityId.SecurityCode, regMsg.Volume, condition.WithdrawInfo, cancellationToken);

			await SendOutMessageAsync(new ExecutionMessage
			{
				DataTypeEx = DataType.Transactions,
				OrderStringId = withdrawId,
				ServerTime = CurrentTime.ConvertToUtc(),
				OriginalTransactionId = regMsg.TransactionId,
				OrderState = OrderStates.Done,
				HasOrderInfo = true,
			}, cancellationToken);

			await PortfolioLookupAsync(null, cancellationToken);
			return;
		}
		default:
			throw new NotSupportedException(LocalizedStrings.OrderUnsupportedType.Put(regMsg.OrderType, regMsg.TransactionId));
	}

	// Determining the order type (market or limit)
	var isMarket = regMsg.OrderType == OrderTypes.Market;
	var price = isMarket ? (decimal?)null : regMsg.Price;

	// Sending the order to the exchange
	var result = await _restClient.RegisterOrder(
		regMsg.TransactionId.To<string>(), regMsg.SecurityId.ToSymbol(),
		regMsg.OrderType.ToNative(), regMsg.Side.ToNative(), price,
		condition?.StopPrice, regMsg.Volume, regMsg.TimeInForce,
		regMsg.TillDate.EnsureToday(), regMsg.Leverage, cancellationToken);

	var orderState = result.Status.ToOrderState();

	// Processing the order registration result
	if (orderState == OrderStates.Failed)
	{
		await SendOutMessageAsync(new ExecutionMessage
		{
			DataTypeEx = DataType.Transactions,
			ServerTime = result.CreationTime,
			OriginalTransactionId = regMsg.TransactionId,
			OrderState = OrderStates.Failed,
			Error = new InvalidOperationException(),
			HasOrderInfo = true,
		}, cancellationToken);
	}
}
```

## Substituição de Ordem

Para substituir uma ordem existente, é implementado o método **ReplaceOrderAsync**. Esse método é chamado ao receber a mensagem [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage).

As principais etapas ao substituir uma ordem:

1. Verificação da possibilidade de substituir a ordem na bolsa.
2. Envio de uma solicitação para substituir a ordem através da API da bolsa.
3. Processamento da resposta da bolsa e envio da mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondente.

```cs
public override async ValueTask ReplaceOrderAsync(OrderReplaceMessage replaceMsg, CancellationToken cancellationToken)
{
	// Sending a request to replace the order
	await _restClient.EditOrder(
		replaceMsg.OldOrderId.To<string>(),
		replaceMsg.Price,
		replaceMsg.Volume,
		cancellationToken);

	// Note: Processing the order replacement result usually occurs
	// in a separate method that is called when receiving an update from the exchange
}
```

### Particularidades da Substituição de Ordem

Ao implementar a substituição de ordens, leve em consideração o protocolo da bolsa. O StockSharp fornece a propriedade [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) para isso.

Se o protocolo da bolsa modifica uma ordem mantendo o identificador antigo, sobrescreva essa propriedade e retorne `true`. Isso informa ao StockSharp que a substituição de uma ordem não deve aguardar um novo identificador da bolsa.

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

Se, ao alterar uma ordem, a antiga for cancelada e uma nova for registrada com um novo identificador da bolsa, então essa propriedade não precisa ser sobrescrita. Por padrão, ela retorna `false`, o que corresponde ao comportamento da maioria das bolsas.

## Cancelamento de Ordem

Para cancelar uma ordem existente, é implementado o método **CancelOrderAsync**. Esse método é chamado ao receber a mensagem [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage).

As principais etapas ao cancelar uma ordem:

1. Verificação da presença do identificador da ordem.
2. Envio de uma solicitação para cancelar a ordem através da API da bolsa.
3. Processamento da resposta da bolsa e envio da mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) correspondente.

```cs
public override async ValueTask CancelOrderAsync(OrderCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	// Checking the presence of the order identifier
	if (cancelMsg.OrderStringId.IsEmpty())
		throw new InvalidOperationException(LocalizedStrings.OrderNoExchangeId.Put(cancelMsg.OriginalTransactionId));

	// Sending a request to cancel the order
	await _restClient.CancelOrder(cancelMsg.OrderStringId, cancellationToken);

	// Note: Processing the order cancellation result usually occurs
	// in a separate method that is called when receiving an update from the exchange
}
```

## Cancelamento em Massa de Ordens

Algumas bolsas suportam a função de cancelamento em massa de ordens, que permite cancelar múltiplas ou todas as ordens ativas com uma única solicitação. Isso pode ser útil para fechar posições rapidamente ou limpar o livro de ofertas sob certas condições de mercado.

Para implementar o cancelamento em massa de ordens no adaptador, normalmente é usado o método **CancelOrderGroupAsync**. Esse método é chamado ao receber a mensagem [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage).

Nem todas as bolsas suportam essa função. Por exemplo, a Coinbase não oferece uma API para cancelamento em massa de ordens. Nesses casos, implemente o cancelamento sequencial de ordens individuais, se necessário.

Abaixo está um exemplo da implementação do método de cancelamento em massa de ordens, retirado do conector [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp), que suporta essa função:

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

Não remova o suporte a esse tipo de comando no construtor do adaptador:

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## Rastreamento do Estado da Ordem

No caso da Coinbase, assim como em algumas outras bolsas modernas, as atualizações do estado das ordens são transmitidas através de uma conexão WebSocket. Isso significa que, após executar operações de negociação (registro, substituição ou cancelamento de uma ordem), não é necessário solicitar imediatamente o novo estado da ordem através da API REST. Em vez disso, o adaptador receberá as atualizações automaticamente através da conexão WebSocket estabelecida.

O processamento dessas atualizações ocorre em um método semelhante a `SessionOnOrderReceived`, que foi discutido na seção sobre [solicitação do estado atual do portfólio e das ordens](portfolio_and_orders_state.md). Esse método é chamado toda vez que a bolsa envia uma atualização sobre o estado da ordem, independentemente de essa atualização ter sido desencadeada por ações do usuário ou por alterações na própria bolsa.

Essa abordagem rastreia os estados das ordens de forma mais eficiente, reduz a carga sobre a API da bolsa e fornece atualizações em tempo real. Ao implementar seu próprio adaptador, estude cuidadosamente a documentação da API da bolsa para que essas atualizações via WebSocket sejam configuradas e tratadas corretamente.

## Tratamento de Erros

Ao executar operações de negociação, trate corretamente os possíveis erros e exceções. Se ocorrer um erro, envie uma mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com a propriedade [Error](xref:StockSharp.Messages.ExecutionMessage.Error) definida.

## Particularidades da Implementação

Ao implementar os métodos de operações de negociação, leve em consideração as particularidades de cada bolsa específica:

- Tipos de ordem suportados (a mercado, limitada, ordens stop, etc.).
- Formato dos identificadores de ordem.
- Particularidades da API da bolsa para trabalhar com ordens.
- Possíveis restrições sobre a frequência de envio de solicitações.
