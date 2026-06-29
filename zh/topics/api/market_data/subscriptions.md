# 订阅

**StockSharp API** 提供基于订阅的数据采集模型。这是一种接收市场数据和交易信息的通用机制。这种方法具有显著的优势：

- **订阅隔离** — 每个订阅独立工作，允许以不同参数运行任意数量的订阅，并行运行时可以选择是否请求历史数据。
- **状态跟踪** — 订阅有特定的状态，允许您控制历史数据是否正在传输，或者订阅是否已切换到实时模式。
- **普遍性** — 无论请求的数据类型如何，处理订阅的代码都是相同的，从而提高了开发效率。

要使用订阅功能，您需要使用 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类。让我们来看一些使用订阅获取各种类型数据的示例。

## 蜡烛订阅示例

```cs
// Create a subscription for 5-minute candles
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// Configure subscription parameters via the MarketData property
	MarketData =
	{
		// Request data for the last 30 days
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null means the subscription will switch to real-time mode after receiving history
		To = null
	}
};

// Processing received candles
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// Process the candle
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// Handling the subscription's transition to online mode
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// Handling subscription errors
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"Subscription error: {error}");
};

// Starting the subscription
_connector.Subscribe(subscription);
```

## 订单簿订阅示例

```cs
// Create a subscription to the order book for the selected instrument
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// Processing received order books
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// Process the order book
	Console.WriteLine($"Order book: {depth.SecurityId}, Time: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// Starting the subscription
_connector.Subscribe(depthSubscription);
```

## Tick交易订阅示例

```cs
// Create a subscription to tick trades for the selected instrument
var tickSubscription = new Subscription(DataType.Ticks, security);

// Processing received ticks
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// Process the tick
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// Starting the subscription
_connector.Subscribe(tickSubscription);
```

## 带蜡烛构建模式配置的订阅示例

```cs
// Subscription to 5-minute candles that will be built from ticks
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// Specify the building mode and data source
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// You can also enable volume profile building
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## 一级订阅示例（基础仪器信息）

```cs
// Creating a subscription for basic instrument information
var level1Subscription = new Subscription(DataType.Level1, security);

// Processing received Level1 data
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// Output Level1 field values
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// Starting the subscription
_connector.Subscribe(level1Subscription);
```

## 取消订阅数据

要停止接收数据，请使用 `UnSubscribe` 方法：

```cs
// Unsubscribe from a specific subscription
_connector.UnSubscribe(subscription);

// Or you can unsubscribe from all subscriptions
foreach (var sub in _connector.Subscriptions)
{
	_connector.UnSubscribe(sub);
}
```

## 订阅状态

订阅可以处于以下状态：

- [SubscriptionStates.Stopped](xref:StockSharp.Messages.SubscriptionStates.Stopped) — 订阅处于非激活状态（已停止或未开始）。
- [SubscriptionStates.Active](xref:StockSharp.Messages.SubscriptionStates.Active) — 订阅处于激活状态，在切换到实时模式或完成之前，可能会传输历史数据。
- [SubscriptionStates.Error](xref:StockSharp.Messages.SubscriptionStates.Error) — 订阅处于非活动状态并且存在错误。
- [SubscriptionStates.Finished](xref:StockSharp.Messages.SubscriptionStates.Finished) — 订阅已完成其工作（所有数据已接收）。
- [SubscriptionStates.Online](xref:StockSharp.Messages.SubscriptionStates.Online) — 订阅已切换到实时模式，仅传输当前数据。
