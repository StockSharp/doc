# テスト設定

このセクションでは、取引ストラテジーをテストするための [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) の主な設定について説明します。

## 基本エミュレーター設定

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - 時刻変更イベントが到着する間隔です。取引ジェネレーターを使用している場合、取引はこの頻度で生成されます。既定値は 1 分です。
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - 送信された注文の最小遅延値です。既定値は TimeSpan.Zero で、送信された注文が取引所に即時受理されることを意味します。
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - 価格がレベルに「触れた」場合に注文を約定させます（この仮定は、ときに過度に「楽観的」であり、現実的なテストではオフにする必要があります）。無効にした場合、指値注文は価格が少なくとも 1 ステップ分「突き抜けた」場合にのみ約定します。このオプションは注文ログモードを除くすべてのモードで動作します。既定では無効です。
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - 注文約定に使用されるローソク足価格です。Middle、Open、High、Low、または Close を使用します。既定値は Middle です。
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - 新規注文登録失敗の割合です。値の範囲は 0（失敗なし）から 100 です。既定では無効（0）です。
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - エミュレーターが注文識別子の生成を開始する初期番号です。
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - エミュレーターが取引識別子の生成を開始する初期番号です。
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - 価格ステップ単位のスプレッドサイズです。ティック取引から板を生成するときに使用されます。既定値は 2 です。
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - ティックから生成される板の最大深度です。既定値は 5 です。
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - ポートフォリオデータの再計算間隔です。TimeSpan.Zero に等しい場合、再計算は実行されません。
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - 注文および取引のタイムスタンプを取引所時刻に変換します。既定では無効です。
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - 取引所のタイムゾーン情報です。
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - 次のセッションの最大価格制限と最小価格制限を定義する、前回取引からの価格オフセットです。既定値は 40% です。
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - 大口注文を登録するときに板へ追加数量を加えます。既定では有効です。
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - 取引セッション状態を確認します。既定では無効です。
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - 現金残高を確認します。既定では無効です。
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - ショートポジションが許可されているか確認します。既定では無効です。
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - 読み込まれた日付が取引日であるか確認します。既定では無効です。
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - 手数料計算ルールです。

## 市場データサブスクリプション

ストラテジーを正しくテストするには、必要な市場データ型へのサブスクリプションを設定する必要があります。ストラテジーをローソク足でテストする場合でも、正しい取引エミュレーションのためにはティック取引へのサブスクリプションが必要です。

```cs
// ティック取引へのサブスクリプションを作成
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

ストラテジーが板データを必要とする場合:

```cs
// 板へのサブスクリプションを作成
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## テスト用の板生成

履歴板が存在しないものの、ストラテジーテストに必要な場合は、板生成を有効にできます。

```cs
// トレンド挙動を持つ板ジェネレーターを作成
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// ジェネレーターにサブスクリプションメッセージを送信
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});
```

### 板ジェネレーター設定

- 板更新間隔（[MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)） - 板は各取引の前に生成されるため、更新はティック取引の到着より頻繁にはできません。

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- 板深度（[MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) および [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)） - 深いほどテストは遅くなります。

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- 板内の現実的なレベル数量を取得するには、[MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) オプションを使用できます。このオプションは、生成される取引数量から最良気配の数量を取得します。

```cs
mdGenerator.UseTradeVolume = true;
```

- レベル数量範囲（[MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) および [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume)）:

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- スプレッド設定 - 生成される最小スプレッドは [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep) に等しくなります。ローソク足から生成する際にスプレッドが広くなりすぎないよう、最良気配間のスプレッドを 5 価格ステップより大きく生成しないことをお勧めします。

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## 包括的なテスト設定例

```cs
// 履歴接続を作成
var connector = new HistoryEmulationConnector();

// 基本パラメーターを設定
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// 履歴データを読み込む
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// ローソク足へのサブスクリプションを作成
var candleSubscription = new Subscription(
	TimeSpan.FromMinutes(5).TimeFrame(),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,
		BuildMode = MarketDataBuildModes.Load
	}
};
connector.Subscribe(candleSubscription);

// 正しいエミュレーションのためにティックへのサブスクリプションを作成
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// 板生成を設定
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId())
{
	Interval = TimeSpan.FromSeconds(1),
	MaxAsksDepth = 5,
	MaxBidsDepth = 5,
	UseTradeVolume = true,
	MinVolume = 1,
	MaxVolume = 100,
	MinSpreadStepCount = 1,
	MaxSpreadStepCount = 5
};

connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});

// データ受信を購読
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// テストを開始
connector.Connect();
```

## イベント処理

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 受信したローソク足を処理
	Console.WriteLine($"Candle: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// 受信したティックを処理
	Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// IOrderBookMessage の拡張メソッドを使用
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// 受信した板を処理
	Console.WriteLine($"Order Book: {orderBook.ServerTime}, Best Bid: {bestBid?.Price}, Best Ask: {bestAsk?.Price}, Middle of Spread: {spreadMiddle}");
	
	// 注文方向によって価格を取得
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Bid Price: {bidPrice}, Ask Price: {askPrice}");
}
```

## 板データの扱い

IOrderBookMessage として板を扱う場合、次の拡張メソッドを使用できます。

```cs
// 最良買気配を取得
var bestBid = orderBook.GetBestBid();

// 最良売気配を取得
var bestAsk = orderBook.GetBestAsk();

// スプレッドの中央を取得
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// 注文方向によって価格を取得
var price = orderBook.GetPrice(Sides.Buy); // または Sides.Sell、またはスプレッド中央の場合は null
```

Level1 データを扱う場合も、スプレッドの中央を取得できます。

```cs
// Level1 メッセージからスプレッドの中央を取得
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

これらの拡張メソッドにより、板データへのアクセスが簡単になり、取引所の気配値を扱う際に、より明確で理解しやすいコードを書けるようになります。
