# ヒストリカルデータの取得

StockSharp API は、ヒストリカルデータを取得するための便利なメカニズムを提供しており、取引ストラテジーのテストと [インジケーター](../indicators.md) の構築の両方に使用できます。

## Connector 経由でヒストリカルデータを取得する

### 接続の設定

ヒストリカルデータを取得するには、まず取引システムへの接続を設定する必要があります。

```cs
// Connector インスタンスを作成します
var connector = new Connector();

// Binance に接続するためのアダプターを追加します
var messageAdapter = new BinanceMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<APIキー>",
	Secret = "<シークレットキー>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);

// 接続します
connector.Connect();
```

接続は、[接続設定ウィンドウ](../graphical_user_interface/connection_settings_window.md) セクションで説明されているように、グラフィカルインターフェイスを使用して設定することもできます。

### ヒストリカルローソク足のサブスクライブ

ヒストリカルローソク足を受信するには、サブスクリプションを作成し、要求するデータのパラメーターを指定する必要があります。

```cs
// 選択した銘柄の 5 分足ローソク足用サブスクリプションを作成します
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		// ヒストリカルデータを取得する期間を指定します
		From = DateTime.Now.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now,
		// 完了済みローソク足のみを受信するフラグを設定します
		IsFinishedOnly = true
	}
};

// ローソク足受信イベントを購読します
connector.CandleReceived += OnCandleReceived;

// サブスクリプションを開始します
connector.Subscribe(subscription);

// ローソク足受信用イベントハンドラー
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足が自分のサブスクリプションに属していることを確認します
	if (subscription != _subscription)
		return;

	// 受信したローソク足を処理します
	Console.WriteLine($"ローソク足を受信: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}, V:{candle.TotalVolume}");

	// チャートに表示するには、次を使用できます:
	// Chart.Draw(_candleElement, candle);
}
```

### チャートでのローソク足の使用

受信したローソク足は、StockSharp 組み込みのグラフィカルコンポーネントを使用してチャートに表示できます。

```cs
// チャート要素を作成して設定します
var chart = new Chart();
var area = new ChartArea();
var candleElement = new ChartCandleElement();

// エリアと要素をチャートに追加します
chart.AddArea(area);
chart.AddElement(area, candleElement, subscription);

// CandleReceived イベントハンドラーでローソク足を描画します
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// ローソク足が自分のサブスクリプションに属していることを確認します
	if (subscription != _subscription)
		return;

	// 完了済みローソク足のみを表示する必要がある場合
	if (candle.State == CandleStates.Finished)
	{
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(candleElement, candle);
		chart.Draw(chartData);
	}
}
```

## その他の種類のヒストリカルデータの取得

同様に、他の種類のヒストリカルデータを取得できます。

### ヒストリカルティックの取得

```cs
var tickSubscription = new Subscription(DataType.Ticks, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
		To = DateTime.Now
	}
};

connector.TickTradeReceived += (subscription, tick) =>
{
	if (subscription == tickSubscription)
		Console.WriteLine($"ティック: {tick.ServerTime}, 価格: {tick.Price}, 数量: {tick.Volume}");
};

connector.Subscribe(tickSubscription);
```

### ヒストリカル板情報の取得

```cs
var depthSubscription = new Subscription(DataType.MarketDepth, security)
{
	MarketData =
	{
		From = DateTime.Now.Subtract(TimeSpan.FromHours(1)),
		To = DateTime.Now
	}
};

connector.OrderBookReceived += (subscription, depth) =>
{
	if (subscription == depthSubscription)
		Console.WriteLine($"板情報: {depth.ServerTime}, 最良買い気配: {depth.GetBestBid()?.Price}, 最良売り気配: {depth.GetBestAsk()?.Price}");
};

connector.Subscribe(depthSubscription);
```

## 関連項目

- [ローソク足](../candles.md)
- [サブスクリプション](subscriptions.md)
- [インジケーター](../indicators.md)
