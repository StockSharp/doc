# Estados das ordens

A API StockSharp fornece a capacidade de receber informação sobre ordens através do mecanismo de subscrição integrado. Tal como nos dados de mercado, a informação de transações usa uma abordagem unificada baseada em [Subscription](xref:StockSharp.BusinessEntities.Subscription).

## Eventos Relacionados com Ordens

[Connector](xref:StockSharp.Algo.Connector) fornece os seguintes eventos para processar informação de ordens:

| Evento | Descrição |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | Evento para receber informação de ordens |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | Evento para falha no registo de ordens |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | Evento para falha no cancelamento de ordens |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | Evento para falha na modificação de ordens |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | Evento para receber informação sobre negócios próprios |

## Enumeração OrderStates

Durante o seu tempo de vida, uma ordem passa pelos seguintes estados:

![Captura de ecrã de Estados das ordens](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - a ordem foi criada no algoritmo de negociação, mas ainda não foi enviada para registo.
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - a ordem foi enviada para registo ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)). O sistema aguarda confirmação da sua aceitação pela bolsa. Se a aceitação for bem-sucedida, o evento [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) será acionado e a ordem passará para o estado [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active). As propriedades [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) e [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime) também serão inicializadas. Se a ordem for rejeitada, o evento [OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) será acionado com uma descrição do erro, e a ordem passará para o estado [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed).
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - a ordem está ativa na bolsa. Essa ordem permanecerá ativa até que todo o seu volume [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume) seja executado ou até ser cancelada forçadamente através de [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)). Se a ordem for parcialmente executada, são acionados os eventos [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) sobre novos negócios da ordem colocada, bem como o evento [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived), que passa uma notificação sobre a alteração do saldo da ordem [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance). Este último evento também será acionado em caso de cancelamento da ordem.
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - a ordem já não está ativa na bolsa (foi totalmente executada ou cancelada).
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - a ordem não foi aceite pela bolsa (ou por um sistema intermediário, como a parte de servidor da plataforma de negociação) por algum motivo.

## Subscrições Automáticas

Por predefinição, [Connector](xref:StockSharp.Algo.Connector) cria automaticamente subscrições para informação de transações ao ligar ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect)). Isto inclui subscrições para:

- Informação de ordens
- Informação de negócios
- Informação de posições
- Pesquisa básica de instrumentos

Exemplo de processamento de um evento de receção de ordem:

```cs
private void InitConnector()
{
	// Assinar evento de recebimento de ordens
	Connector.OrderReceived += OnOrderReceived;
	
	// Assinar evento de recebimento de negociações próprias
	Connector.OwnTradeReceived += OnOwnTradeReceived;
	
	// Assinar evento de falha no registro de ordem
	Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
	// Processar ordem recebida
	_ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// Importante! Verifique se a ordem pertence à assinatura atual
	// para evitar processamento duplicado
	if (subscription == _myOrdersSubscription)
	{
		// Processamento adicional para a assinatura específica
		Console.WriteLine($"Ordem: {order.TransactionId}, Estado: {order.State}");
	}
}
```

## Criação Manual de Subscrições de Ordens

Em alguns casos, pode ser necessário solicitar explicitamente informação sobre ordens. Para isso, pode criar subscrições separadas:

```cs
// Criar assinatura para ordens de uma carteira específica
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
	TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// Manipulador para recebimento de ordens
Connector.OrderReceived += (subscription, order) =>
{
	if (subscription == ordersSubscription)
	{
		Console.WriteLine($"Ordem: {order.TransactionId}, Estado: {order.State}, Carteira: {order.Portfolio.Name}");
	}
};

// Iniciar a assinatura
Connector.Subscribe(ordersSubscription);
```

## Verificar o Estado da Ordem

São usados métodos de extensão para determinar o estado atual de uma ordem:

```cs
// Verificar estado da ordem
Order order = ...; // ordem recebida

// A ordem foi cancelada
bool isCanceled = order.IsCanceled();

// A ordem foi totalmente executada
bool isMatched = order.IsMatched();

// A ordem foi parcialmente executada
bool isPartiallyMatched = order.IsMatchedPartially();

// Pelo menos parte da ordem foi executada
bool isNotEmpty = order.IsMatchedEmpty();

// Obter volume executado
decimal matchedVolume = order.GetMatchedVolume();
```

## Abordagem Avançada: Trabalhar com Várias Subscrições

Em cenários complexos, pode ser necessário trabalhar com várias subscrições de ordens em simultâneo. Neste caso, é importante processar corretamente os eventos para evitar duplicação:

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
	// Assinatura para ordens da primeira carteira
	_portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);
	
	// Assinatura para ordens da segunda carteira
	_portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);
	
	// Manipulador comum para recebimento de ordens
	Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;
	
	// Iniciar assinaturas
	Connector.Subscribe(_portfolio1OrdersSubscription);
	Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
	// Determinar a qual assinatura a ordem pertence
	if (subscription == _portfolio1OrdersSubscription)
	{
		// Processar ordens da primeira carteira
	}
	else if (subscription == _portfolio2OrdersSubscription)
	{
		// Processar ordens da segunda carteira
	}
}
```

> [!NOTE]
> Esta abordagem avançada com várias subscrições de ordens deve ser usada apenas em casos excecionais, quando o mecanismo de subscrição padrão é insuficiente.

## Natureza Assíncrona das Transações

O envio de transações (registo, substituição ou cancelamento de ordens) é efetuado de forma assíncrona. Isto permite que o programa de negociação não espere pela confirmação da bolsa e continue a funcionar, acelerando a reação a alterações na situação de mercado.

Para acompanhar o estado de uma ordem, é necessário subscrever os eventos correspondentes:
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) para receber atualizações do estado da ordem
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) para tratar erros de registo

## Ver Também

- [Subscrições](../market_data/subscriptions.md)
- [Estados da Ordem](orders_states.md)
- [Criar uma Nova Ordem](create_new_order.md)
- [Cancelar Ordens](order_cancel.md)
