# Informações sobre Portfólios e Ordens

Ao criar seu próprio adaptador para trabalhar com uma bolsa, você precisa implementar métodos para solicitar o estado atual do portfólio e das ordens. Esses métodos são chamados ao receber as mensagens [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) e [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage), respectivamente.

## Solicitando o Estado do Portfólio

Para solicitar o estado do portfólio, é implementado o método **PortfolioLookupAsync**. Esse método normalmente executa as seguintes ações:

1. Envia uma confirmação de recebimento da solicitação usando [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Verifica se a solicitação é uma assinatura ou cancelamento de assinatura usando a propriedade [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe).
3. Em caso de assinatura:
  - Envia uma mensagem [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) com informações sobre o portfólio.
  - Solicita os saldos atuais da conta na bolsa.
  - Para cada conta, cria e envia uma mensagem [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) com informações sobre a posição.
4. Envia uma mensagem sobre o resultado da assinatura usando [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	var transId = lookupMsg.TransactionId;

	// Send confirmation of receiving the request
	await SendSubscriptionReplyAsync(transId, cancellationToken);

	if (!lookupMsg.IsSubscribe)
		return;

	// Send a message with information about the portfolio
	await SendOutMessageAsync(new PortfolioMessage
	{
		PortfolioName = PortfolioName,
		BoardCode = BoardCodes.Coinbase,
		OriginalTransactionId = transId,
	}, cancellationToken);

	// Request current account balances
	var accounts = await _restClient.GetAccounts(cancellationToken);

	foreach (var account in accounts)
	{
		// For each account, create and send a message with information about the position
		await SendOutMessageAsync(new PositionChangeMessage
		{
			PortfolioName = PortfolioName,
			SecurityId = new SecurityId
			{
				SecurityCode = account.Currency,
				BoardCode = BoardCodes.Coinbase,
			},
			ServerTime = CurrentTime.ConvertToUtc(),
		}
		.TryAdd(PositionChangeTypes.CurrentValue, (decimal)account.Available, true)
		.TryAdd(PositionChangeTypes.BlockedValue, (decimal)account.Hold, true), cancellationToken);
	}

	// Send a message about successful completion of the subscription
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

## Solicitando o Estado das Ordens

Para solicitar o estado das ordens, é implementado o método **OrderStatusAsync**. Esse método normalmente executa as seguintes ações:

1. Envia uma confirmação de recebimento da solicitação usando [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)).
2. Verifica se a solicitação é uma assinatura ou cancelamento de assinatura usando a propriedade [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe).
3. Em caso de assinatura:
  - Solicita a lista de ordens atuais na bolsa.
  - Para cada ordem, cria e envia uma mensagem [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) com informações sobre a ordem.
  - Se necessário, configura uma assinatura para receber atualizações de ordens em tempo real.
4. Envia uma mensagem sobre o resultado da assinatura usando [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
	// Send confirmation of receiving the request
	await SendSubscriptionReplyAsync(statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsSubscribe)
		return;

	// Request the list of current orders
	var orders = await _restClient.GetOrders(cancellationToken);

	foreach (var order in orders)
		await ProcessOrder(order, statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsHistoryOnly())
	{
		// Set up a subscription to receive order updates in real time
		await _socketClient.SubscribeOrders(cancellationToken);
	}

	// Send a message about successful completion of the subscription
	await SendSubscriptionResultAsync(statusMsg, cancellationToken);
}

private async ValueTask ProcessOrder(Order order, long originTransId, CancellationToken cancellationToken)
{
	if (!long.TryParse(order.ClientOrderId, out var transId))
		return;

	var state = order.Status.ToOrderState();

	// Create and send a message with information about the order
	await SendOutMessageAsync(new ExecutionMessage
	{
		ServerTime = originTransId == 0 ? CurrentTime.ConvertToUtc() : order.CreationTime,
		DataTypeEx = DataType.Transactions,
		SecurityId = order.Product.ToStockSharp(),
		TransactionId = originTransId == 0 ? 0 : transId,
		OriginalTransactionId = originTransId,
		OrderState = state,
		Error = state == OrderStates.Failed ? new InvalidOperationException() : null,
		OrderType = order.Type.ToOrderType(),
		Side = order.Side.ToSide(),
		OrderStringId = order.Id,
		OrderPrice = order.Price?.ToDecimal() ?? 0,
		OrderVolume = order.Size?.ToDecimal(),
		TimeInForce = order.TimeInForce.ToTimeInForce(),
		Balance = (decimal?)order.LeavesQuantity,
		HasOrderInfo = true,
	}, cancellationToken);
}
```

## Processamento de Atualizações em Tempo Real

Para processar atualizações do estado das ordens em tempo real, normalmente é implementado um método separado, que é chamado ao receber os eventos correspondentes do cliente WebSocket:

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// Process the received order update
	// OriginTransId = 0, since this is a real-time update, not a response to a specific request
	await ProcessOrder(order, 0, cancellationToken);
}
```
