# 投资组合和订单信息

为交易所创建自己的适配器时，需要实现用于请求当前投资组合状态和订单状态的方法。收到 [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) 和 [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) 消息时，会分别调用这些方法。

## 请求投资组合状态

要请求投资组合状态，需要实现 **PortfolioLookupAsync** 方法。该方法通常执行以下操作：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) 发送已收到请求的确认。
2. 使用 [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe) 属性检查请求是订阅还是取消订阅。
3. 如果是订阅：
  - 发送包含投资组合信息的 [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) 消息。
  - 从交易所请求当前账户余额。
  - 对每个账户，创建并发送包含持仓信息的 [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) 消息。
4. 使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) 发送订阅结果消息。

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	var transId = lookupMsg.TransactionId;

	// 发送已收到请求的确认
	await SendSubscriptionReplyAsync(transId, cancellationToken);

	if (!lookupMsg.IsSubscribe)
		return;

	// 发送包含投资组合信息的消息
	await SendOutMessageAsync(new PortfolioMessage
	{
		PortfolioName = PortfolioName,
		BoardCode = BoardCodes.Coinbase,
		OriginalTransactionId = transId,
	}, cancellationToken);

	// 请求当前账户余额
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

	// 发送订阅成功完成消息
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

## 请求订单状态

要请求订单状态，需要实现 **OrderStatusAsync** 方法。此方法通常执行以下操作：

1. 使用 [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) 发送已收到请求的确认。
2. 使用 [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe) 属性检查请求是订阅还是取消订阅。
3. 如果是订阅：
  - 从交易所请求当前订单列表。
  - 对每个订单，创建并发送包含订单信息的 [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) 消息。
  - 如有必要，建立实时接收订单更新的订阅。
4. 使用 [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) 发送订阅结果消息。

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
	// 发送已收到请求的确认
	await SendSubscriptionReplyAsync(statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsSubscribe)
		return;

	// 请求当前订单列表
	var orders = await _restClient.GetOrders(cancellationToken);

	foreach (var order in orders)
		await ProcessOrder(order, statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsHistoryOnly())
	{
		// 设置实时接收订单更新的订阅
		await _socketClient.SubscribeOrders(cancellationToken);
	}

	// 发送订阅成功完成消息
	await SendSubscriptionResultAsync(statusMsg, cancellationToken);
}

private async ValueTask ProcessOrder(Order order, long originTransId, CancellationToken cancellationToken)
{
	if (!long.TryParse(order.ClientOrderId, out var transId))
		return;

	var state = order.Status.ToOrderState();

	// 创建并发送包含订单信息的消息
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

为了处理实时订单状态更新，通常会实现一个单独的方法，在 WebSocket 客户端收到相应事件时调用：

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// 处理收到的订单更新
	// OriginTransId = 0, since this is a real-time update, not a response to a specific request
	await ProcessOrder(order, 0, cancellationToken);
}
```
