# 测试设置

本节描述了用于测试交易策略的 [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) 的主要设置。

## 基本模拟器设置

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - 时间变更事件到达的间隔。如果使用交易生成器，交易将以此频率生成。默认值为1分钟。
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - 提交订单的最小延迟值。默认值为 TimeSpan.Zero，这意味着交易所即时接受提交的订单。
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - 如果价格“触及”该水平，则满足订单（这种假设有时过于“乐观”，在实际测试中应关闭）。如果禁用，限价单仅在价格至少“穿过”一步时才会被执行。此选项在除订单日志模式外的所有模式下都可用。默认情况下为禁用。

## 市场数据订阅

为了进行正确的策略测试，有必要订阅所需的市场数据类型。即使策略是在蜡烛图上测试的，为了正确的交易模拟，也需要订阅逐笔交易数据：

```cs
// Create a subscription to tick trades
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

如果策略需要订单簿数据：

```cs
// Create a subscription to order books
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## 用于测试的订单簿生成

如果没有历史订单簿，但策略测试需要它们，你可以启用订单簿生成：

```cs
// Create an order book generator with trend behavior
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// Send a subscription message to the generator
_connector.MarketDataAdapter.SendInMessage(new GeneratorMessage
{
	IsSubscribe = true,
	Generator = mdGenerator
});
```

### 订单簿生成器设置

- 订单簿更新间隔（[MarketDataGenerator.Interval](xref:StockSharp.Algo.Testing.MarketDataGenerator.Interval)） - 更新不能比成交数据到达更频繁，因为订单簿是在每笔交易之前生成的：

```cs
mdGenerator.Interval = TimeSpan.FromSeconds(1);
```

- 订单簿深度（[MarketDepthGenerator.MaxBidsDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxBidsDepth) 和 [MarketDepthGenerator.MaxAsksDepth](xref:StockSharp.Algo.Testing.MarketDepthGenerator.MaxAsksDepth)）——越深，测试越慢：

```cs
mdGenerator.MaxAsksDepth = 1; 
mdGenerator.MaxBidsDepth = 1;
```

- 要在订单簿中获得真实水平的成交量，您可以使用 [MarketDepthGenerator.UseTradeVolume](xref:StockSharp.Algo.Testing.MarketDepthGenerator.UseTradeVolume) 选项，该选项从正在生成的交易量中获取最佳报价的成交量：

```cs
mdGenerator.UseTradeVolume = true;
```

- 音量范围 ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) 和 [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume))：

```cs
mdGenerator.MinVolume = 1;
mdGenerator.MaxVolume = 1;
```

- 点差设置 - 最小生成点差等于 [Security.PriceStep](xref:StockSharp.BusinessEntities.Security.PriceStep)。建议不要在最佳报价之间生成超过5个价位的点差，以免从K线生成时，点差过大:

```cs
mdGenerator.MinSpreadStepCount = 1;
mdGenerator.MaxSpreadStepCount = 5;
```

## 全面测试配置示例

```cs
// Create a historical connection
var connector = new HistoryEmulationConnector();

// Configure basic parameters
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// Load historical data
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// Create a subscription to candles
var candleSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
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

// Create a subscription to ticks for correct emulation
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// Configure order book generation
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

// Subscribe to data reception
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// Start testing
connector.Connect();
```

## 事件处理

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Processing received candles
	Console.WriteLine($"Candle: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// Processing received ticks
	Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// Using extension methods for IOrderBookMessage
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// Processing received order books
	Console.WriteLine($"Order Book: {orderBook.ServerTime}, Best Bid: {bestBid?.Price}, Best Ask: {bestAsk?.Price}, Middle of Spread: {spreadMiddle}");
	
	// Getting price by order side
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Bid Price: {bidPrice}, Ask Price: {askPrice}");
}
```

## 处理订单簿数据

在使用 IOrderBookMessage 处理订单簿时，你可以使用以下扩展方法：

```cs
// Get the best bid
var bestBid = orderBook.GetBestBid();

// Get the best ask
var bestAsk = orderBook.GetBestAsk();

// Get the middle of the spread
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// Get the price by order side
var price = orderBook.GetPrice(Sides.Buy); // or Sides.Sell, or null for the middle of the spread
```

在处理 Level1 数据时，你也可以获取买卖价差的中间值：

```cs
// Get the middle of the spread from a Level1 message
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

这些扩展方法简化了对订单簿数据的访问，并且在处理交易所报价时可以让你的代码更清晰、更易理解。