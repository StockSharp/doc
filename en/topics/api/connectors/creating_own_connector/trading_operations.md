# Trading Operations

When creating your own adapter for working with an exchange, it is necessary to implement methods for performing trading operations, such as registering, replacing, and canceling orders. These methods are called when receiving the corresponding messages from the StockSharp core.

## Order Registration

To register a new order, the **RegisterOrderAsync** method is implemented. This method is called when receiving the [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) message.

The main steps when registering an order:

1. Checking the order type and additional conditions.
2. Converting order parameters to a format understood by the exchange.
3. Sending a request to register the order through the exchange API.
4. Processing the response from the exchange and sending the corresponding [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message.

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

			SendOutMessage(new ExecutionMessage
			{
				DataTypeEx = DataType.Transactions,
				OrderStringId = withdrawId,
				ServerTime = CurrentTime.ConvertToUtc(),
				OriginalTransactionId = regMsg.TransactionId,
				OrderState = OrderStates.Done,
				HasOrderInfo = true,
			});

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
		SendOutMessage(new ExecutionMessage
		{
			DataTypeEx = DataType.Transactions,
			ServerTime = result.CreationTime,
			OriginalTransactionId = regMsg.TransactionId,
			OrderState = OrderStates.Failed,
			Error = new InvalidOperationException(),
			HasOrderInfo = true,
		});
	}
}
```

## Order Replacement

To replace an existing order, the **ReplaceOrderAsync** method is implemented. This method is called when receiving the [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) message.

The main steps when replacing an order:

1. Checking the possibility of replacing the order on the exchange.
2. Sending a request to replace the order through the exchange API.
3. Processing the response from the exchange and sending the corresponding [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message.

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

### Specifics of Order Replacement

When implementing the order replacement method, it is important to consider the specifics of the exchange protocol. For this, StockSharp provides the [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) property.

If the exchange protocol assumes changing an order while retaining its old identifier, it is necessary to override this property and return `true`. This indicates to StockSharp that when replacing an order, it is not necessary to expect a new identifier from the exchange.

public override bool IsReplaceCommandEditCurrent => true;

If, when changing an order, the old one is canceled and a new one is registered with a new exchange identifier, then this property does not need to be overridden. By default, it returns `false`, which corresponds to the behavior of most exchanges.

## Order Cancellation

To cancel an existing order, the **CancelOrderAsync** method is implemented. This method is called when receiving the [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) message.

The main steps when canceling an order:

1. Checking the presence of the order identifier.
2. Sending a request to cancel the order through the exchange API.
3. Processing the response from the exchange and sending the corresponding [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message.

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

## Mass Order Cancellation

Some exchanges support the mass order cancellation function, which allows canceling multiple or all active orders with a single request. This can be useful for quickly closing positions or clearing the order book under certain market conditions.

To implement mass order cancellation in the adapter, the **CancelOrderGroupAsync** method is usually used. This method is called when receiving the [OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) message.

It is worth noting that not all exchanges support this function. For example, Coinbase does not provide an API for mass order cancellation. In such cases, it may be necessary to implement sequential cancellation of individual orders.

Below is an example of the implementation of the mass order cancellation method, taken from the [BitStamp](https://github.com/StockSharp/StockSharp/tree/master/Connectors/BitStamp) connector, which supports this function:

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

It is important not to forget to remove the deletion of support for this command type from the adapter constructor:

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## Tracking Order State

In the case of Coinbase, as well as some other modern exchanges, order state updates are broadcast through a WebSocket connection. This means that after performing trading operations (registration, replacement, or cancellation of an order), there is no need to immediately request the new order state through the REST API. Instead, the adapter will receive updates automatically through the established WebSocket connection.

Processing these updates occurs in a method similar to `SessionOnOrderReceived`, which was discussed in the section on [requesting the current state of the portfolio and orders](portfolio_and_orders_state.md). This method is called every time the exchange sends an update about the order state, regardless of whether this update was triggered by user actions or changes on the exchange itself.

This approach allows for more efficient tracking of order states, reduces the load on the exchange API, and ensures receiving updates in real time. However, when implementing your own adapter, it is necessary to carefully study the API documentation of the exchange being used in order to properly configure and handle these WebSocket updates.

## Error Handling

When performing trading operations, it is important to correctly handle possible errors and exceptions. In case of an error, it is necessary to send an [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message with the [Error](xref:StockSharp.Messages.ExecutionMessage.Error) property set.

## Implementation Specifics

When implementing methods for working with trading operations, it is necessary to take into account the specifics of a particular exchange:

- Supported order types (market, limit, stop orders, etc.).
- Format of order identifiers.
- Specifics of the exchange API for working with orders.
- Possible restrictions on the frequency of sending requests.