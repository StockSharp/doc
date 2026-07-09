# 测试设置

本节描述了用于测试交易策略的 [HistoryEmulationConnector](xref:StockSharp.Algo.Testing.HistoryEmulationConnector) 的主要设置。

## 基本模拟器设置

- [MarketTimeChangedInterval](xref:StockSharp.Algo.Testing.HistoryEmulationConnector.MarketTimeChangedInterval) - 时间变更事件到达的间隔。如果使用交易生成器，交易将以此频率生成。默认值为1分钟。
- [MarketEmulatorSettings.Latency](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Latency) - 提交订单的最小延迟值。默认值为 TimeSpan.Zero，这意味着交易所即时接受提交的订单。
- [MarketEmulatorSettings.MatchOnTouch](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MatchOnTouch) - 如果价格“触及”该水平，则满足订单（这种假设有时过于“乐观”，在实际测试中应关闭）。如果禁用，限价单仅在价格至少“穿过”一步时才会被执行。此选项在除订单日志模式外的所有模式下都可用。默认情况下为禁用。
- [MarketEmulatorSettings.CandlePrice](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CandlePrice) - 用于订单成交的 K线价格：Middle、Open、High、Low 或 Close。默认值为 Middle。
- [MarketEmulatorSettings.Failing](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.Failing) - 新订单注册失败的百分比。取值范围为 0（无失败）到 100。默认禁用（0）。
- [MarketEmulatorSettings.InitialOrderId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialOrderId) - 模拟器生成订单标识符时使用的初始编号。
- [MarketEmulatorSettings.InitialTradeId](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.InitialTradeId) - 模拟器生成成交标识符时使用的初始编号。
- [MarketEmulatorSettings.SpreadSize](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.SpreadSize) - 以价格步长表示的价差大小。从逐笔成交生成订单簿时使用。默认值为 2。
- [MarketEmulatorSettings.MaxDepth](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.MaxDepth) - 从 ticks 生成订单簿时的最大深度。默认值为 5。
- [MarketEmulatorSettings.PortfolioRecalcInterval](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PortfolioRecalcInterval) - 投资组合数据重新计算间隔。如果等于 TimeSpan.Zero，则不执行重新计算。
- [MarketEmulatorSettings.ConvertTime](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.ConvertTime) - 将订单和成交时间戳转换为交易所时间。默认禁用。
- [MarketEmulatorSettings.TimeZone](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.TimeZone) - 交易所时区信息。
- [MarketEmulatorSettings.PriceLimitOffset](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.PriceLimitOffset) - 根据上一笔成交定义下一交易时段最高价和最低价限制的价格偏移。默认值为 40%。
- [MarketEmulatorSettings.IncreaseDepthVolume](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.IncreaseDepthVolume) - 注册大额订单时向订单簿增加额外数量。默认启用。
- [MarketEmulatorSettings.CheckTradingState](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradingState) - 检查交易时段状态。默认禁用。
- [MarketEmulatorSettings.CheckMoney](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckMoney) - 检查资金余额。默认禁用。
- [MarketEmulatorSettings.CheckShortable](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckShortable) - 检查是否允许做空。默认禁用。
- [MarketEmulatorSettings.CheckTradableDates](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CheckTradableDates) - 检查加载的日期是否为交易日。默认禁用。
- [MarketEmulatorSettings.CommissionRules](xref:StockSharp.Algo.Testing.MarketEmulatorSettings.CommissionRules) - 佣金计算规则。

## 市场数据订阅

为了进行正确的策略测试，有必要订阅所需的市场数据类型。即使策略是在K线上测试的，为了正确的交易模拟，也需要订阅逐笔交易数据：

```cs
// 创建 tick 成交订阅
var tickSubscription = new Subscription(DataType.Ticks, security);
_connector.Subscribe(tickSubscription);
```

如果策略需要订单簿数据：

```cs
// 创建订单簿订阅
var depthSubscription = new Subscription(DataType.MarketDepth, security);
_connector.Subscribe(depthSubscription);
```

## 用于测试的订单簿生成

如果没有历史订单簿，但策略测试需要它们，你可以启用订单簿生成：

```cs
// 创建具有趋势行为的订单簿生成器
var mdGenerator = new TrendMarketDepthGenerator(security.ToSecurityId());

// 向生成器发送订阅消息
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

- 成交量范围 ([MarketDataGenerator.MinVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MinVolume) 和 [MarketDataGenerator.MaxVolume](xref:StockSharp.Algo.Testing.MarketDataGenerator.MaxVolume))：

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
// 创建历史连接
var connector = new HistoryEmulationConnector();

// 配置基本参数
connector.MarketTimeChangedInterval = TimeSpan.FromSeconds(10);
connector.EmulationAdapter.Emulator.Settings.Latency = TimeSpan.FromMilliseconds(100);
connector.EmulationAdapter.Emulator.Settings.MatchOnTouch = false;

// 加载历史数据
var storage = new StorageRegistry();
var security = new Security { Id = "AAPL", PriceStep = 0.01m };

// 创建 K线订阅
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

// 创建逐笔成交订阅以便正确仿真
var tickSubscription = new Subscription(DataType.Ticks, security);
connector.Subscribe(tickSubscription);

// 配置订单簿生成
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

// 订阅数据接收
connector.CandleReceived += OnCandleReceived;
connector.TickTradeReceived += OnTickReceived;
connector.OrderBookReceived += OnOrderBookReceived;

// 开始测试
connector.Connect();
```

## 事件处理

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 处理收到的K线
	Console.WriteLine($"Candle: {candle.OpenTime}, O:{candle.OpenPrice}, H:{candle.HighPrice}, L:{candle.LowPrice}, C:{candle.ClosePrice}");
}

private void OnTickReceived(Subscription subscription, ITickTradeMessage tick)
{
	// 处理收到的逐笔成交
	Console.WriteLine($"Tick: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
}

private void OnOrderBookReceived(Subscription subscription, IOrderBookMessage orderBook)
{
	// 使用 IOrderBookMessage 的扩展方法
	var bestBid = orderBook.GetBestBid();
	var bestAsk = orderBook.GetBestAsk();
	var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);
	
	// 处理收到的订单簿
	Console.WriteLine($"Order Book: {orderBook.ServerTime}, Best Bid: {bestBid?.Price}, Best Ask: {bestAsk?.Price}, Middle of Spread: {spreadMiddle}");
	
	// 按订单方向获取价格
	var bidPrice = orderBook.GetPrice(Sides.Buy);
	var askPrice = orderBook.GetPrice(Sides.Sell);
	
	Console.WriteLine($"Bid Price: {bidPrice}, Ask Price: {askPrice}");
}
```

## 处理订单簿数据

在使用 IOrderBookMessage 处理订单簿时，你可以使用以下扩展方法：

```cs
// 获取最佳买价
var bestBid = orderBook.GetBestBid();

// 获取最佳卖价
var bestAsk = orderBook.GetBestAsk();

// 获取价差中点
var spreadMiddle = orderBook.GetSpreadMiddle(Security.PriceStep);

// 按订单方向获取价格
var price = orderBook.GetPrice(Sides.Buy); // or Sides.Sell, or null for the middle of the spread
```

在处理 Level1 数据时，你也可以获取买卖价差的中间值：

```cs
// 从 Level1 消息获取价差中点
var spreadMiddle = level1.GetSpreadMiddle(Security.PriceStep);
```

这些扩展方法简化了对订单簿数据的访问，并且在处理交易所报价时可以让你的代码更清晰、更易理解。
