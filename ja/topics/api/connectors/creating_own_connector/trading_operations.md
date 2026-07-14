# 取引操作

取引所向けの独自アダプターを作成する場合、注文の登録、差し替え、キャンセルといった取引操作を実行するメソッドを実装します。これらのメソッドは、アダプターが StockSharp コアから対応するメッセージを受信したときに呼び出されます。

## 注文登録

新しい注文を登録するには、**RegisterOrderAsync** メソッドを実装します。このメソッドは、[OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) メッセージを受信したときに呼び出されます。

注文登録時の主な手順:

1. 注文タイプと追加条件を確認します。
2. 注文パラメーターを取引所が理解できる形式に変換します。
3. 取引所 API を通じて注文登録要求を送信します。
4. 取引所からの応答を処理し、対応する [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージを送信します。

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
			// 条件付き注文の処理。たとえば資金の出金
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

	// 注文タイプ（成行または指値）の判定
	var isMarket = regMsg.OrderType == OrderTypes.Market;
	var price = isMarket ? (decimal?)null : regMsg.Price;
	
	// 取引所へ注文を送信
	var result = await _restClient.RegisterOrder(
		regMsg.TransactionId.To<string>(), regMsg.SecurityId.ToSymbol(),
		regMsg.OrderType.ToNative(), regMsg.Side.ToNative(), price,
		condition?.StopPrice, regMsg.Volume, regMsg.TimeInForce,
		regMsg.TillDate.EnsureToday(), regMsg.Leverage, cancellationToken);

	var orderState = result.Status.ToOrderState();

	// 注文登録結果の処理
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

## 注文差し替え

既存の注文を差し替えるには、**ReplaceOrderAsync** メソッドを実装します。このメソッドは、[OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) メッセージを受信したときに呼び出されます。

注文差し替え時の主な手順:

1. 取引所で注文を差し替え可能か確認します。
2. 取引所 API を通じて注文差し替え要求を送信します。
3. 取引所からの応答を処理し、対応する [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージを送信します。

```cs
public override async ValueTask ReplaceOrderAsync(OrderReplaceMessage replaceMsg, CancellationToken cancellationToken)
{
	// 注文差し替え要求を送信
	await _restClient.EditOrder(
		replaceMsg.OldOrderId.To<string>(), 
		replaceMsg.Price, 
		replaceMsg.Volume, 
		cancellationToken);
	
	// 注: 注文差し替え結果の処理は通常、
	// 取引所から更新を受信したときに呼び出される別メソッドで行われます
}
```

### 注文差し替えの詳細

注文差し替えを実装する際は、取引所プロトコルを考慮してください。StockSharp はこのために [MessageAdapter.IsReplaceCommandEditCurrent](xref:StockSharp.Messages.MessageAdapter.IsReplaceCommandEditCurrent) プロパティを提供しています。

取引所プロトコルが古い識別子を保持したまま注文を変更する場合、このプロパティをオーバーライドして `true` を返します。これにより、注文差し替え時に取引所から新しい識別子が返されるのを待つべきではないことを StockSharp に伝えます。

```cs
public override bool IsReplaceCommandEditCurrent => true;
```

注文を変更するときに古い注文がキャンセルされ、新しい取引所識別子を持つ新しい注文が登録される場合、このプロパティをオーバーライドする必要はありません。既定では `false` を返し、これは多くの取引所の動作に対応しています。

## 注文キャンセル

既存の注文をキャンセルするには、**CancelOrderAsync** メソッドを実装します。このメソッドは、[OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) メッセージを受信したときに呼び出されます。

注文キャンセル時の主な手順:

1. 注文識別子が存在することを確認します。
2. 取引所 API を通じて注文キャンセル要求を送信します。
3. 取引所からの応答を処理し、対応する [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージを送信します。

```cs
public override async ValueTask CancelOrderAsync(OrderCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	// 注文識別子が存在することを確認
	if (cancelMsg.OrderStringId.IsEmpty())
		throw new InvalidOperationException(LocalizedStrings.OrderNoExchangeId.Put(cancelMsg.OriginalTransactionId));

	// 注文キャンセル要求を送信
	await _restClient.CancelOrder(cancelMsg.OrderStringId, cancellationToken);

	// 注: 注文キャンセル結果の処理は通常、
	// 取引所から更新を受信したときに呼び出される別メソッドで行われます
}
```

## 注文の一括キャンセル

一部の取引所は、1 回の要求で複数またはすべてのアクティブ注文をキャンセルできる一括注文キャンセル機能をサポートしています。これは、特定の市場状況でポジションをすばやく閉じたり、注文板をクリアしたりする場合に役立ちます。

アダプターで注文の一括キャンセルを実装するには、通常 **CancelOrderGroupAsync** メソッドを使用します。このメソッドは、[OrderGroupCancelMessage](xref:StockSharp.Messages.OrderGroupCancelMessage) メッセージを受信したときに呼び出されます。

すべての取引所がこの機能をサポートしているわけではありません。たとえば Coinbase は、一括注文キャンセル用の API を提供していません。このような場合、必要であれば個別注文の逐次キャンセルを実装します。

以下は、この機能をサポートする [BitStamp](https://github.com/StockSharp/Connectors/tree/main/BitStamp) コネクターから取得した、一括注文キャンセルメソッドの実装例です。

```cs
public override async ValueTask CancelOrderGroupAsync(OrderGroupCancelMessage cancelMsg, CancellationToken cancellationToken)
{
	await _httpClient.CancelAllOrders(cancellationToken);
}
```

アダプターのコンストラクターで、このコマンドタイプのサポートを削除しないでください。

```cs
//this.RemoveSupportedMessage(MessageTypes.OrderGroupCancel);
```

## 注文状態の追跡

Coinbase の場合、およびその他の一部の現代的な取引所では、注文状態の更新は WebSocket 接続を通じて配信されます。つまり、取引操作（注文の登録、差し替え、キャンセル）を実行した後に、REST API を通じて新しい注文状態をすぐに要求する必要はありません。代わりに、確立済みの WebSocket 接続を通じて、アダプターが更新を自動的に受信します。

これらの更新の処理は、[ポートフォリオと注文の現在状態の要求](portfolio_and_orders_state.md)に関するセクションで説明した `SessionOnOrderReceived` に似たメソッドで行われます。このメソッドは、その更新がユーザー操作によって発生したものか、取引所自体の変更によって発生したものかに関係なく、取引所が注文状態に関する更新を送信するたびに呼び出されます。

このアプローチでは、注文状態をより効率的に追跡でき、取引所 API への負荷を減らし、リアルタイム更新を提供できます。独自のアダプターを実装する際は、これらの WebSocket 更新が正しく設定および処理されるように、取引所 API ドキュメントを慎重に確認してください。

## エラー処理

取引操作を実行する際は、発生し得るエラーや例外を正しく処理してください。エラーが発生した場合は、[ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) メッセージの [Error](xref:StockSharp.Messages.ExecutionMessage.Error) プロパティを設定して送信します。

## 実装上の詳細

取引操作メソッドを実装する際は、特定の取引所の仕様を考慮してください。

- サポートされる注文タイプ（成行、指値、ストップ注文など）。
- 注文識別子の形式。
- 注文を扱う取引所 API の仕様。
- 要求送信頻度に関する制限の可能性。
