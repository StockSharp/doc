# Information about Portfolios and Orders

When creating your own adapter for working with an exchange, it is necessary to implement methods for requesting the current state of the portfolio and orders. These methods are called when receiving [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) and [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) messages respectively.

## Requesting Portfolio State

To request the portfolio state, the **PortfolioLookupAsync** method is implemented. This method usually performs the following actions:

1. Sends a confirmation of receiving the request using [IMessageAdapter.SendSubscriptionReply](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionReply(long)).
2. Checks whether the request is a subscription or unsubscription using the [PortfolioLookupMessage.IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe) property.
3. In case of a subscription:
  - Sends a [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) message with information about the portfolio.
  - Requests the current account balances from the exchange.
  - For each account, creates and sends a [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) message with information about the position.
4. Sends a message about the subscription result using [IMessageAdapter.SendSubscriptionResult](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionResult(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
    var transId = lookupMsg.TransactionId;

    // Send confirmation of receiving the request
    SendSubscriptionReply(transId);

    if (!lookupMsg.IsSubscribe)
        return;

    // Send a message with information about the portfolio
    SendOutMessage(new PortfolioMessage
    {
        PortfolioName = PortfolioName,
        BoardCode = BoardCodes.Coinbase,
        OriginalTransactionId = transId,
    });

    // Request current account balances
    var accounts = await _restClient.GetAccounts(cancellationToken);

    foreach (var account in accounts)
    {
        // For each account, create and send a message with information about the position
        SendOutMessage(new PositionChangeMessage
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
        .TryAdd(PositionChangeTypes.BlockedValue, (decimal)account.Hold, true));
    }

    // Send a message about successful completion of the subscription
    SendSubscriptionResult(lookupMsg);
}
```

## Requesting Orders State

To request the orders state, the **OrderStatusAsync** method is implemented. This method usually performs the following actions:

1. Sends a confirmation of receiving the request using [IMessageAdapter.SendSubscriptionReply](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionReply(long)).
2. Checks whether the request is a subscription or unsubscription using the [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe) property.
3. In case of a subscription:
  - Requests the list of current orders from the exchange.
  - For each order, creates and sends an [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) message with information about the order.
  - If necessary, sets up a subscription to receive order updates in real time.
4. Sends a message about the subscription result using [IMessageAdapter.SendSubscriptionResult](xref:StockSharp.Messages.IMessageAdapter.SendSubscriptionResult(StockSharp.Messages.ISubscriptionMessage)).

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
    // Send confirmation of receiving the request
    SendSubscriptionReply(statusMsg.TransactionId);

    if (!statusMsg.IsSubscribe)
        return;

    // Request the list of current orders
    var orders = await _restClient.GetOrders(cancellationToken);

    foreach (var order in orders)
        ProcessOrder(order, statusMsg.TransactionId);

    if (!statusMsg.IsHistoryOnly())
    {
        // Set up a subscription to receive order updates in real time
        await _socketClient.SubscribeOrders(cancellationToken);
    }

    // Send a message about successful completion of the subscription
    SendSubscriptionResult(statusMsg);
}

private void ProcessOrder(Order order, long originTransId)
{
    if (!long.TryParse(order.ClientOrderId, out var transId))
        return;

    var state = order.Status.ToOrderState();

    // Create and send a message with information about the order
    SendOutMessage(new ExecutionMessage
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
    });
}
```

## Processing Real-Time Updates

To process real-time order state updates, a separate method is usually implemented, which is called when receiving corresponding events from the WebSocket client:

```cs
private void SessionOnOrderReceived(Order order)
{
    // Process the received order update
    // OriginTransId = 0, since this is a real-time update, not a response to a specific request
    ProcessOrder(order, 0);
}
```