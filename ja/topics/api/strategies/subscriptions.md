# 戦略における市場データ購読

StockSharp では、戦略は市場データを受信するためにサブスクリプション機構を使用します。この方式は、取引戦略でデータを取得するための主要かつ推奨される方法です。

## サブスクリプションの基本

戦略内のサブスクリプションは、一般的な [StockSharp サブスクリプション機構](../market_data/subscriptions.md) に基づいています。これにより、あらゆる種類の市場データを取得するための一元的で統一された方法が提供されます。

## 戦略内でのサブスクリプション作成

戦略の [OnStarted2](xref:StockSharp.Algo.Strategies.Strategy.OnStarted2(System.DateTime)) メソッドで、必要なデータ用のサブスクリプションを作成して開始できます。

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// DataType を通じて 5 分足のサブスクリプションを直接作成
	var subscription = new Subscription(
		DataType.TimeFrame(TimeSpan.FromMinutes(5)),
		Security);
	
	// 追加パラメーターが必要な場合は、サブスクリプションを設定できます
	subscription.From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(7));
	
	// 受信したローソク足を処理するルールを作成
	Connector
		.WhenCandlesFinished(subscription)
		.Do(ProcessCandle)
		.Apply(this);
	
	// サブスクリプションを開始
	Connector.Subscribe(subscription);
}
```

この例では、`DataType` と `Security` を受け取る便利なコンストラクターを使用して、5 分足のサブスクリプションを作成しています。必要に応じて、履歴期間などのサブスクリプションパラメーターを追加で設定できます。

## 戦略でサブスクリプションを使用する利点

戦略でサブスクリプションを使用すると、[Strategy.Connector](xref:StockSharp.Algo.Strategies.Strategy.Connector) イベントへ直接購読する場合と比べて、いくつかの利点があります。

1. **分離** - 各サブスクリプションは独立して動作するため、相互干渉なしに、異なる銘柄について異なる種類のデータを受信できます。これにより、並行して実行されている他の戦略向けのデータを戦略が受け取ることも防げます。コネクターイベントへ直接購読する場合は、他の戦略の情報を除外するために追加でデータをフィルターする必要があります。

2. **状態管理** - サブスクリプションには明確な状態 ([SubscriptionStates](xref:StockSharp.Messages.SubscriptionStates)) があり、履歴データを現在受信中か、サブスクリプションがすでにオンラインモードへ移行したかを正確に判断できます。

3. **戦略状態の自動制御** - 戦略はすべてのサブスクリプションの状態を自動的に追跡し、すべてのサブスクリプションがオンラインになった場合にのみオンラインモード ([IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline)) へ移行します。

4. **コードの統一性** - サブスクリプションは、要求されるデータの種類に依存しない統一されたアプローチを使用します。

5. **ルールとの統合** - サブスクリプションは、ルールを通じて戦略の [イベントモデル](event_model.md) と簡単に統合できます。

6. **自動サブスクリプション管理** - 戦略が停止すると、すべてのサブスクリプションが自動的にキャンセルされ、リソースが解放されます。

7. **履歴データのサポート** - リアルタイムデータへ移行する前に履歴データを読み込めます。

## サブスクリプション状態の監視

戦略は、動作モードを制御するために、すべてのサブスクリプションの状態を自動的に追跡します。

```cs
private void CheckRefreshOnlineState()
{
	bool nowOnline = ProcessState == ProcessStates.Started;

	if (nowOnline)
		nowOnline = _subscriptions.CachedKeys
			.Where(s => !s.SubscriptionMessage.IsHistoryOnly())
			.All(s => s.State == SubscriptionStates.Online);
	
	// 戦略の IsOnline 状態を更新
	IsOnline = nowOnline;
}
```

[Strategy.IsOnline](xref:StockSharp.Algo.Strategies.Strategy.IsOnline) プロパティは、すべての戦略サブスクリプションが [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) 状態へ移行した場合にのみ `true` になります。これにより、戦略は現在の市場データを扱っている瞬間を把握できます。

## サブスクリプションの種類

戦略では、さまざまな種類の市場データに対してサブスクリプションを使用できます。

```cs
// ローソク足へのサブスクリプション
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	Security);

// 板情報へのサブスクリプション
var depthSubscription = new Subscription(
	DataType.MarketDepth,
	Security);

// ティック取引へのサブスクリプション
var tickSubscription = new Subscription(
	DataType.Ticks,
	Security);

// Level1 (最良気配値やその他の基本情報) へのサブスクリプション
var level1Subscription = new Subscription(
	DataType.Level1,
	Security);
```

## ルールによるサブスクリプションデータの処理

サブスクリプションを通じて届くデータを処理するには、[ルール](event_model.md) を使用することを推奨します。

```cs
// ローソク足へのサブスクリプション
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), Security);

// 受信したローソク足を処理するルールを作成
Connector
	.WhenCandlesFinished(subscription)  // 完成したローソク足を受信したときにルールを有効化
	.Do(ProcessCandle)                   // 処理メソッドを呼び出し
	.Apply(this);                        // ルールを戦略へ適用

// サブスクリプションを開始
Connector.Subscribe(subscription);
```

上の例では、完成したローソク足を受信するたびに `ProcessCandle` メソッドを呼び出すルールを作成しています。

## 履歴データの要求

戦略は、[Strategy.HistorySize](xref:StockSharp.Algo.Strategies.Strategy.HistorySize) プロパティを通じて履歴読み込み期間を自動的に設定します。

```cs
// 履歴読み込み期間を 30 日に設定
strategy.HistorySize = TimeSpan.FromDays(30);
```

サブスクリプションを作成するとき、明示的に指定されていない場合、戦略は履歴を読み込むために `From` パラメーターを自動的に設定します。

## サブスクリプションのキャンセル

サブスクリプションは、[UnSubscribe](xref:StockSharp.BusinessEntities.ISubscriptionProvider.UnSubscribe(StockSharp.BusinessEntities.Subscription)) メソッドを呼び出すことで手動でキャンセルできます。

```cs
// サブスクリプションをキャンセル
Connector.UnSubscribe(subscription);
```

戦略を停止するとき、[UnsubscribeOnStop](xref:StockSharp.Algo.Strategies.Strategy.UnsubscribeOnStop) パラメーターが `true` (既定値) に設定されている場合、すべてのサブスクリプションは自動的にキャンセルされます。

## 関連項目

- [市場データ購読](../market_data/subscriptions.md)
- [イベントモデル](event_model.md)
- [戦略プラットフォーム互換性](compatibility.md)

