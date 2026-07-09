# サブスクリプション

**StockSharp API** は、サブスクリプションに基づくデータ取得モデルを提供します。これは、市場データと取引情報の両方を受信するための汎用メカニズムです。このアプローチには大きな利点があります。

- **サブスクリプションの分離** — 各サブスクリプションは独立して動作し、異なるパラメーターを持つ任意の数のサブスクリプションを並行して実行できます（ヒストリー要求の有無を問わず）。
- **状態追跡** — サブスクリプションには特定の状態があり、ヒストリカルデータが現在流れているか、サブスクリプションがリアルタイムモードに切り替わったかを制御できます。
- **汎用性** — 要求するデータの種類に関係なく、サブスクリプションを扱うコードは同じであるため、開発をより効率化できます。

サブスクリプションを扱うには、[Subscription](xref:StockSharp.BusinessEntities.Subscription) クラスを使用する必要があります。さまざまな種類のデータを取得するためにサブスクリプションを使用する例を見てみましょう。

## キャンドルサブスクリプションの例

```cs
// 5 分足キャンドルのサブスクリプションを作成します
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// MarketData プロパティを介してサブスクリプションパラメーターを設定します
	MarketData =
	{
		// 直近 30 日間のデータを要求します
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null は、ヒストリー受信後にサブスクリプションがリアルタイムモードへ切り替わることを意味します
		To = null
	}
};

// 受信したキャンドルを処理します
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// キャンドルを処理します
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// サブスクリプションのオンラインモードへの移行を処理します
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// サブスクリプションエラーを処理します
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"サブスクリプションエラー: {error}");
};

// サブスクリプションを開始します
_connector.Subscribe(subscription);
```

## 板情報サブスクリプションの例

```cs
// 選択した銘柄の板情報へのサブスクリプションを作成します
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// 受信した板情報を処理します
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// 板情報を処理します
	Console.WriteLine($"板情報: {depth.SecurityId}, 時刻: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// サブスクリプションを開始します
_connector.Subscribe(depthSubscription);
```

## ティック取引サブスクリプションの例

```cs
// 選択した銘柄のティック取引へのサブスクリプションを作成します
var tickSubscription = new Subscription(DataType.Ticks, security);

// 受信したティックを処理します
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// ティックを処理します
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// サブスクリプションを開始します
_connector.Subscribe(tickSubscription);
```

## キャンドル構築モード設定付きサブスクリプションの例

```cs
// ティックから構築される 5 分足キャンドルへのサブスクリプション
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// 構築モードとデータソースを指定します
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// 出来高プロファイルの構築も有効にできます
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## Level1 サブスクリプションの例（基本的な銘柄情報）

```cs
// 基本的な銘柄情報のサブスクリプションを作成します
var level1Subscription = new Subscription(DataType.Level1, security);

// 受信した Level1 データを処理します
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// Level1 フィールド値を出力します
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// サブスクリプションを開始します
_connector.Subscribe(level1Subscription);
```

## データの購読解除

データの受信を停止するには、`UnSubscribe` メソッドを使用します。

```cs
// 特定のサブスクリプションを購読解除します
_connector.UnSubscribe(subscription);

// または、すべてのサブスクリプションを購読解除できます
foreach (var sub in _connector.Subscriptions)
{
	_connector.UnSubscribe(sub);
}
```

## サブスクリプション状態

サブスクリプションは次の状態を取ることができます。

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — サブスクリプションは非アクティブです（停止済み、または開始されていません）。
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — サブスクリプションはアクティブで、リアルタイムモードへの切り替えまたは完了までヒストリカルデータを送信する場合があります。
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — サブスクリプションは非アクティブで、エラー状態です。
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — サブスクリプションは処理を完了しました（すべてのデータを受信済み）。
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — サブスクリプションはリアルタイムモードに切り替わっており、現在のデータのみを送信します。
