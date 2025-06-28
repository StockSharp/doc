# Orders states

StockSharp API provides the ability to receive information about orders through the built-in subscription mechanism. As with market data, transaction information uses a unified approach based on [Subscription](xref:StockSharp.BusinessEntities.Subscription).

## Order-related Events

[Connector](xref:StockSharp.Algo.Connector) provides the following events for processing order information:

| Event | Description |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | Event for receiving order information |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | Event for order registration failure |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | Event for order cancellation failure |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | Event for order modification failure |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | Event for receiving information about own trades |

## OrderStates enum

During its lifetime, an order goes through the following states:

![OrderStates](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - the order has been created in the trading algorithm but has not yet been sent for registration.
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - the order has been sent for registration ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)). The system is waiting for confirmation of its acceptance from the exchange. If the acceptance is successful, the [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) event will be triggered, and the order will be transferred to the [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) state. The [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) and [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime) properties will also be initialized. If the order is rejected, the [OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) event will be triggered with an error description, and the order will be transferred to the [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) state.
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - the order is active on the exchange. Such an order will remain active until its entire volume [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume) is executed, or it is forcibly canceled through [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order). If the order is partially executed, the [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) events about new trades for the placed order are triggered, as well as the [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) event, which passes a notification about the change in the order balance [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance). The latter event will also be triggered in case of order cancellation.
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - the order is no longer active on the exchange (it was fully executed or canceled).
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - the order was not accepted by the exchange (or an intermediate system, such as the server part of the trading platform) for some reason.

## Automatic Subscriptions

By default, [Connector](xref:StockSharp.Algo.Connector) automatically creates subscriptions for transaction information when connecting ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect)). This includes subscriptions to:

- Order information
- Trade information
- Position information
- Basic instrument lookup

Example of handling an order reception event:

```cs
private void InitConnector()
{
	// Subscribe to order reception event
	Connector.OrderReceived += OnOrderReceived;
	
	// Subscribe to own trade reception event
	Connector.OwnTradeReceived += OnOwnTradeReceived;
	
	// Subscribe to order registration failure event
	Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
	// Process the received order
	_ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// Important! Check if the order belongs to the current subscription
	// to avoid duplicate processing
	if (subscription == _myOrdersSubscription)
	{
		// Additional processing for the specific subscription
		Console.WriteLine($"Order: {order.TransactionId}, State: {order.State}");
	}
}
```

## Manual Creation of Order Subscriptions

In some cases, you may need to explicitly request information about orders. For this, you can create separate subscriptions:

```cs
// Create a subscription for orders of a specific portfolio
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
	TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// Handler for receiving orders
Connector.OrderReceived += (subscription, order) =>
{
	if (subscription == ordersSubscription)
	{
		Console.WriteLine($"Order: {order.TransactionId}, State: {order.State}, Portfolio: {order.Portfolio.Name}");
	}
};

// Start the subscription
Connector.Subscribe(ordersSubscription);
```

## Checking Order Status

Extension methods are used to determine the current state of an order:

```cs
// Check order status
Order order = ...; // received order

// Is the order canceled
bool isCanceled = order.IsCanceled();

// Is the order fully executed
bool isMatched = order.IsMatched();

// Is the order partially executed
bool isPartiallyMatched = order.IsMatchedPartially();

// Is at least part of the order executed
bool isNotEmpty = order.IsMatchedEmpty();

// Get the executed volume
decimal matchedVolume = order.GetMatchedVolume();
```

## Advanced Approach: Working with Multiple Subscriptions

In complex scenarios, you may need to work with multiple order subscriptions simultaneously. In this case, it's important to properly handle events to avoid duplication:

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
	// Subscription for orders of the first portfolio
	_portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);
	
	// Subscription for orders of the second portfolio
	_portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);
	
	// Common handler for receiving orders
	Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;
	
	// Start subscriptions
	Connector.Subscribe(_portfolio1OrdersSubscription);
	Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
	// Determine which subscription the order belongs to
	if (subscription == _portfolio1OrdersSubscription)
	{
		// Process orders of the first portfolio
	}
	else if (subscription == _portfolio2OrdersSubscription)
	{
		// Process orders of the second portfolio
	}
}
```

> [!NOTE]
> Such an advanced approach with multiple subscriptions to orders should be used only in exceptional cases when the standard subscription mechanism is insufficient.

## Asynchronous Nature of Transactions

Transaction sending (registration, replacement, or cancellation of orders) is performed asynchronously. This allows the trading program not to wait for confirmation from the exchange, but to continue working, which speeds up the reaction to changes in the market situation.

To track the status of an order, you need to subscribe to the corresponding events:
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) for receiving order status updates
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) for handling registration errors

## See Also

- [Subscriptions](../market_data/subscriptions.md)
- [Order States](orders_states.md)
- [Creating a New Order](create_new_order.md)
- [Canceling Orders](order_cancel.md)