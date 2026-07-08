# 订阅

**StockSharp API** 提供基于订阅的数据采集模型。这是一种接收市场数据和交易信息的通用机制。这种方法具有显著的优势：

- **订阅隔离** — 每个订阅独立工作，允许以不同参数运行任意数量的订阅，并行运行时可以选择是否请求历史数据。
- **状态跟踪** — 订阅有特定的状态，允许您控制历史数据是否正在传输，或者订阅是否已切换到实时模式。
- **普遍性** — 无论请求的数据类型如何，处理订阅的代码都是相同的，从而提高了开发效率。

要使用订阅功能，您需要使用 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类。让我们来看一些使用订阅获取各种类型数据的示例。

## K线订阅示例

```cs
// 创建 5 分钟 K线订阅
var subscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	// 通过 MarketData 属性配置订阅参数
	MarketData =
	{
		// 请求最近 30 天的数据
		From = DateTimeOffset.Now.Subtract(TimeSpan.FromDays(30)),
		// null 表示订阅在接收历史数据后会切换到实时模式
		To = null
	}
};

// 处理接收到的 K线
_connector.CandleReceived += (sub, candle) =>
{
	if (sub != subscription)
		return;

	// 处理 K线
	Console.WriteLine($"Candle: {candle.OpenTime} - O:{candle.OpenPrice} H:{candle.HighPrice} L:{candle.LowPrice} C:{candle.ClosePrice} V:{candle.TotalVolume}");
};

// 处理订阅切换到在线模式
_connector.SubscriptionOnline += (sub) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine("Subscription switched to real-time mode");
};

// 处理订阅错误
_connector.SubscriptionFailed += (sub, error, isSubscribe) =>
{
	if (sub != subscription)
		return;

	Console.WriteLine($"Subscription error: {error}");
};

// 启动订阅
_connector.Subscribe(subscription);
```

## 订单簿订阅示例

```cs
// 为所选交易品种创建订单簿订阅
var depthSubscription = new Subscription(DataType.MarketDepth, security);

// 处理接收到的订单簿
_connector.OrderBookReceived += (sub, depth) =>
{
	if (sub != depthSubscription)
		return;

	// 处理订单簿
	Console.WriteLine($"Order book: {depth.SecurityId}, Time: {depth.ServerTime}");
	Console.WriteLine($"Bids: {depth.Bids.Count}, Asks: {depth.Asks.Count}");
};

// 启动订阅
_connector.Subscribe(depthSubscription);
```

## Tick交易订阅示例

```cs
// 为所选交易品种创建 tick 成交订阅
var tickSubscription = new Subscription(DataType.Ticks, security);

// 处理接收到的 tick
_connector.TickTradeReceived += (sub, tick) =>
{
	if (sub != tickSubscription)
		return;

	// 处理 tick
	Console.WriteLine($"Tick: {tick.SecurityId}, Time: {tick.ServerTime}, Price: {tick.Price}, Volume: {tick.Volume}");
};

// 启动订阅
_connector.Subscribe(tickSubscription);
```

## 带K线构建模式配置的订阅示例

```cs
// 从 tick 构建的 5 分钟 K线订阅
var candleSubscription = new Subscription(DataType.TimeFrame(TimeSpan.FromMinutes(5)), security)
{
	MarketData =
	{
		// 指定构建模式和数据源
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks,
		// 也可以启用成交量分布构建
		IsCalcVolumeProfile = true,
	}
};

_connector.Subscribe(candleSubscription);
```

## 一级订阅示例（基础交易品种信息）

```cs
// 创建基础交易品种信息订阅
var level1Subscription = new Subscription(DataType.Level1, security);

// 处理接收到的 Level1 数据
_connector.Level1Received += (sub, level1) =>
{
	if (sub != level1Subscription)
		return;

	Console.WriteLine($"Level1: {level1.SecurityId}, Time: {level1.ServerTime}");

	// 输出 Level1 字段值
	foreach (var pair in level1.Changes)
	{
		Console.WriteLine($"Field: {pair.Key}, Value: {pair.Value}");
	}
};

// 启动订阅
_connector.Subscribe(level1Subscription);
```

## 取消订阅数据

要停止接收数据，请使用 `UnSubscribe` 方法：

```cs
// 取消特定订阅
_connector.UnSubscribe(subscription);

// 或者可以取消所有订阅
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
