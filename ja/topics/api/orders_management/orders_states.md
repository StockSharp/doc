# 注文状態

StockSharp API は、組み込みのサブスクリプション メカニズムを通じて注文に関する情報を受信する機能を提供します。マーケット データと同様に、トランザクション情報は [Subscription](xref:StockSharp.BusinessEntities.Subscription) に基づく統一されたアプローチを使用します。

## 注文関連イベント

[Connector](xref:StockSharp.Algo.Connector) は、注文情報を処理するために次のイベントを提供します。

| イベント | 説明 |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | 注文情報を受信するためのイベント |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | 注文登録失敗のイベント |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | 注文キャンセル失敗のイベント |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | 注文変更失敗のイベント |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | 自分の取引に関する情報を受信するためのイベント |

## OrderStates enum

注文は、その存続期間中に次の状態をたどります。

![注文状態 のスクリーンショット](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - 注文は取引アルゴリズム内で作成されていますが、まだ登録のために送信されていません。
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - 注文は登録のために送信されています ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order))。システムは取引所からの受理確認を待機しています。受理が成功すると、[OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) イベントが発生し、注文は [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) 状態へ移行します。[Order.Id](xref:StockSharp.BusinessEntities.Order.Id) および [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime) プロパティも初期化されます。注文が拒否された場合、[OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) イベントがエラー説明付きで発生し、注文は [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) 状態へ移行します。
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - 注文は取引所でアクティブです。このような注文は、その全数量 [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume) が約定するか、[CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)) によって強制的にキャンセルされるまでアクティブのままです。注文が部分約定した場合、発注済み注文に対する新しい取引について [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) イベントが発生し、さらに [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived) イベントも発生します。このイベントは注文残高 [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance) の変化に関する通知を渡します。後者のイベントは注文キャンセル時にも発生します。
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - 注文は取引所でアクティブではなくなっています (完全に約定したか、キャンセルされました)。
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - 注文は何らかの理由で取引所 (または取引プラットフォームのサーバー部分などの中間システム) に受理されませんでした。

## 自動サブスクリプション

既定では、[Connector](xref:StockSharp.Algo.Connector) は接続時にトランザクション情報のサブスクリプションを自動的に作成します ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect))。これには次へのサブスクリプションが含まれます。

- 注文情報
- 取引情報
- ポジション情報
- 基本的な銘柄検索

注文受信イベントを処理する例:

```cs
private void InitConnector()
{
	// 注文受信イベントをサブスクライブします
	Connector.OrderReceived += OnOrderReceived;
	
	// 自分の取引の受信イベントをサブスクライブします
	Connector.OwnTradeReceived += OnOwnTradeReceived;
	
	// 注文登録失敗イベントをサブスクライブします
	Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
	// 受信した注文を処理します
	_ordersWindow.OrderGrid.Orders.TryAdd(order);
	
	// 重要: 重複処理を避けるため、注文が現在のサブスクリプションに
	// 属しているかを確認します
	if (subscription == _myOrdersSubscription)
	{
		// 特定のサブスクリプションに対する追加処理
		Console.WriteLine($"注文: {order.TransactionId}, 状態: {order.State}");
	}
}
```

## 注文サブスクリプションの手動作成

場合によっては、注文に関する情報を明示的に要求する必要があります。このために、個別のサブスクリプションを作成できます。

```cs
// 特定のポートフォリオの注文用サブスクリプションを作成します
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
	TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// 注文を受信するためのハンドラー
Connector.OrderReceived += (subscription, order) =>
{
	if (subscription == ordersSubscription)
	{
		Console.WriteLine($"注文: {order.TransactionId}, 状態: {order.State}, ポートフォリオ: {order.Portfolio.Name}");
	}
};

// サブスクリプションを開始します
Connector.Subscribe(ordersSubscription);
```

## 注文ステータスの確認

注文の現在状態を判定するには、拡張メソッドを使用します。

```cs
// 注文ステータスを確認します
Order order = ...; // 受信した注文

// 注文はキャンセルされているか
bool isCanceled = order.IsCanceled();

// 注文は完全に約定しているか
bool isMatched = order.IsMatched();

// 注文は部分約定しているか
bool isPartiallyMatched = order.IsMatchedPartially();

// 注文の少なくとも一部が約定しているか
bool isNotEmpty = order.IsMatchedEmpty();

// 約定済み数量を取得します
decimal matchedVolume = order.GetMatchedVolume();
```

## 高度なアプローチ: 複数サブスクリプションの操作

複雑なシナリオでは、複数の注文サブスクリプションを同時に扱う必要がある場合があります。この場合、重複を避けるためにイベントを適切に処理することが重要です。

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
	// 1 つ目のポートフォリオの注文用サブスクリプション
	_portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);
	
	// 2 つ目のポートフォリオの注文用サブスクリプション
	_portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);
	
	// 注文を受信するための共通ハンドラー
	Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;
	
	// サブスクリプションを開始します
	Connector.Subscribe(_portfolio1OrdersSubscription);
	Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
	// 注文がどのサブスクリプションに属しているかを判定します
	if (subscription == _portfolio1OrdersSubscription)
	{
		// 1 つ目のポートフォリオの注文を処理します
	}
	else if (subscription == _portfolio2OrdersSubscription)
	{
		// 2 つ目のポートフォリオの注文を処理します
	}
}
```

> [!NOTE]
> 複数の注文サブスクリプションを使用するこのような高度なアプローチは、標準のサブスクリプション メカニズムでは不十分な例外的な場合にのみ使用してください。

## トランザクションの非同期性

トランザクションの送信 (注文の登録、差し替え、またはキャンセル) は非同期に実行されます。これにより、取引プログラムは取引所からの確認を待たずに動作を継続できるため、市場状況の変化への反応が速くなります。

注文のステータスを追跡するには、対応するイベントにサブスクライブする必要があります。
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived): 注文ステータス更新の受信
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived): 登録エラーの処理

## 関連項目

- [サブスクリプション](../market_data/subscriptions.md)
- [注文状態](orders_states.md)
- [新しい注文の作成](create_new_order.md)
- [注文のキャンセル](order_cancel.md)
