# 关于投资组合和订单的信息

在为交易所创建自己的适配器时，有必要实现用于请求当前投资组合和订单状态的方法。这些方法分别在接收到 [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) 和 [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) 消息时被调用。

## 请求组合状态

要请求投资组合状态，实现了 **PortfolioLookupAsync** 方法。该方法通常执行以下操作：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception) 发送收到请求的确认。
2. 使用 [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe) 属性检查请求是订阅还是取消订阅。
3. 在订阅的情况下：
  - 发送包含投资组合信息的[PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage)消息。
  - 请求从交易所获取当前账户余额。
  - 对于每个账户，创建并发送包含头寸信息的 [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) 消息。
4. 使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage) 发送关于订阅结果的消息。

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

## 请求订单状态

要请求订单状态，实现了 **OrderStatusAsync** 方法。此方法通常执行以下操作：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception) 发送收到请求的确认。
2. 使用 [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe) 属性检查请求是订阅还是退订。
3. 在订阅的情况下：
  - 请求交易所当前订单的列表。
  - 对于每个订单，创建并发送包含订单信息的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。
  - 如有必要，建立订阅以实时接收订单更新。
4. 使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage) 发送关于订阅结果的消息。

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

## 处理实时更新

为了处理实时订单状态更新，通常会实现一个单独的方法，当从 WebSocket 客户端接收到相应事件时会调用该方法：

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// Process the received order update
	// OriginTransId = 0, since this is a real-time update, not a response to a specific request
	await ProcessOrder(order, 0, cancellationToken);
}
```