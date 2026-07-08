# ポートフォリオと注文に関する情報

取引所と連携する独自のアダプターを作成する場合、ポートフォリオと注文の現在状態を要求するメソッドを実装する必要があります。これらのメソッドは、それぞれ [PortfolioLookupMessage](xref:StockSharp.Messages.PortfolioLookupMessage) および [OrderStatusMessage](xref:StockSharp.Messages.OrderStatusMessage) メッセージを受信したときに呼び出されます。

## ポートフォリオ状態の要求

ポートフォリオ状態を要求するには、**PortfolioLookupAsync** メソッドを実装します。このメソッドは通常、次の処理を行います。

1. [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) を使用して、要求を受信したことの確認を送信します。
2. [IsSubscribe](xref:StockSharp.Messages.PortfolioLookupMessage.IsSubscribe) プロパティを使用して、その要求が購読か購読解除かを確認します。
3. 購読の場合:
  - ポートフォリオに関する情報を含む [PortfolioMessage](xref:StockSharp.Messages.PortfolioMessage) メッセージを送信します。
  - 取引所から現在の口座残高を要求します。
  - 各口座について、ポジションに関する情報を含む [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) メッセージを作成して送信します。
4. [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) を使用して、購読結果に関するメッセージを送信します。

```cs
public override async ValueTask PortfolioLookupAsync(PortfolioLookupMessage lookupMsg, CancellationToken cancellationToken)
{
	var transId = lookupMsg.TransactionId;

	// 要求を受信したことの確認を送信
	await SendSubscriptionReplyAsync(transId, cancellationToken);

	if (!lookupMsg.IsSubscribe)
		return;

	// ポートフォリオに関する情報を含むメッセージを送信
	await SendOutMessageAsync(new PortfolioMessage
	{
		PortfolioName = PortfolioName,
		BoardCode = BoardCodes.Coinbase,
		OriginalTransactionId = transId,
	}, cancellationToken);

	// 現在の口座残高を要求
	var accounts = await _restClient.GetAccounts(cancellationToken);

	foreach (var account in accounts)
	{
		// 各口座について、ポジションに関する情報を含むメッセージを作成して送信
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

	// 購読が正常に完了したことを示すメッセージを送信
	await SendSubscriptionResultAsync(lookupMsg, cancellationToken);
}
```

## 注文状態の要求

注文状態を要求するには、**OrderStatusAsync** メソッドを実装します。このメソッドは通常、次の処理を行います。

1. [SendSubscriptionReplyAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionReplyAsync(System.Int64,System.Exception)) を使用して、要求を受信したことの確認を送信します。
2. [OrderStatusMessage.IsSubscribe](xref:StockSharp.Messages.OrderStatusMessage.IsSubscribe) プロパティを使用して、その要求が購読か購読解除かを確認します。
3. 購読の場合:
  - 取引所から現在の注文一覧を要求します。
  - 各注文について、注文に関する情報を含む [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージを作成して送信します。
  - 必要に応じて、リアルタイムで注文更新を受信するための購読を設定します。
4. [SendSubscriptionResultAsync](xref:StockSharp.Messages.MessageAdapter.SendSubscriptionResultAsync(StockSharp.Messages.ISubscriptionMessage)) を使用して、購読結果に関するメッセージを送信します。

```cs
public override async ValueTask OrderStatusAsync(OrderStatusMessage statusMsg, CancellationToken cancellationToken)
{
	// 要求を受信したことの確認を送信
	await SendSubscriptionReplyAsync(statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsSubscribe)
		return;

	// 現在の注文一覧を要求
	var orders = await _restClient.GetOrders(cancellationToken);

	foreach (var order in orders)
		await ProcessOrder(order, statusMsg.TransactionId, cancellationToken);

	if (!statusMsg.IsHistoryOnly())
	{
		// リアルタイムで注文更新を受信するための購読を設定
		await _socketClient.SubscribeOrders(cancellationToken);
	}

	// 購読が正常に完了したことを示すメッセージを送信
	await SendSubscriptionResultAsync(statusMsg, cancellationToken);
}

private async ValueTask ProcessOrder(Order order, long originTransId, CancellationToken cancellationToken)
{
	if (!long.TryParse(order.ClientOrderId, out var transId))
		return;

	var state = order.Status.ToOrderState();

	// 注文に関する情報を含むメッセージを作成して送信
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

## リアルタイム更新の処理

リアルタイムの注文状態更新を処理するには、通常、WebSocket クライアントから対応するイベントを受信したときに呼び出される別のメソッドを実装します。

```cs
private async ValueTask SessionOnOrderReceived(Order order, CancellationToken cancellationToken)
{
	// 受信した注文更新を処理
	// これは特定の要求への応答ではなくリアルタイム更新であるため、OriginTransId = 0
	await ProcessOrder(order, 0, cancellationToken);
}
```
