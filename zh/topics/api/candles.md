# K线

[S#](../api.md) 支持以下类型的K线：

- [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) - 基于时间间隔（时间周期）的K线。既可以设置常用周期（分钟、小时、日），也可以设置自定义周期，例如 21 秒、4.5 分钟等。
- [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) - 价格范围K线。当出现价格超出允许范围的成交时，会创建新K线。每次都根据第一笔成交的价格确定允许范围。
- [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) - 在成交总量超过指定限制前持续形成K线。如果新成交使数量超过允许值，则该成交会计入下一根新K线。
- [TickCandleMessage](xref:StockSharp.Messages.TickCandleMessage) - 与 [VolumeCandleMessage](xref:StockSharp.Messages.VolumeCandleMessage) 类似，但使用成交笔数而不是成交量作为限制。
- [PnFCandleMessage](xref:StockSharp.Messages.PnFCandleMessage) - 点数图K线（X-O 图）。
- [RenkoCandleMessage](xref:StockSharp.Messages.RenkoCandleMessage) - 砖形图（Renko）K线。

K线的使用方法可参阅 *Samples\/02\_Candles\/01\_Realtime* 文件夹中的示例。

下图分别展示 [TimeFrameCandleMessage](xref:StockSharp.Messages.TimeFrameCandleMessage) 和 [RangeCandleMessage](xref:StockSharp.Messages.RangeCandleMessage) 图表：

![sample timeframecandles](../../images/sample_timeframecandles.png)

![sample rangecandles](../../images/sample_rangecandles.png)

## 开始获取数据

1. 要获取K线，请使用 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类创建订阅：

```cs
// Create a subscription to 5-minute candles
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // Data type with timeframe specification
	security)  // Instrument
{
	// Configure additional parameters through the MarketData property
	MarketData = 
	{
		// Period for which we request historical data (last 30 days)
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. 要接收K线，请订阅 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件。出现可供处理的新值时，该事件会发出通知：

```cs
// Subscribe to the candle reception event
_connector.CandleReceived += OnCandleReceived;

// Candle reception event handler
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Here subscription is the subscription object we created
	// candle - the received candle
	
	// Check if the candle belongs to our subscription
	if (subscription == _candleSubscription)
	{
		// Draw the candle on the chart
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> [Chart](xref:StockSharp.Xaml.Charting.Chart) 图形组件用于显示K线。

3. 接下来，通过 [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) 方法启动订阅：

```cs
// Start the subscription
_connector.Subscribe(subscription);
```

之后会开始调用 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件。

4. [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件不仅会在出现新K线时调用，当前K线发生变化时也会调用。

如果只需要显示**已完成**的K线，应检查所接收K线的 [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) 属性：

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// Check if the candle belongs to our subscription
	if (subscription != _candleSubscription)
		return;
	
	// Check if the candle is completed
	if (candle.State == CandleStates.Finished) 
	{
		// Create data for drawing
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// Draw the candle on the chart
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. 还可以为订阅配置其他参数：

- **K线构建模式** - 指定请求现成数据，还是使用其他数据类型构建K线：

```cs
// Request only ready-made data
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// Only build from another data type
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// Request ready-made data, and if not available - build
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **K线构建数据源** - 如果无法直接获取K线，指定使用哪种数据类型进行构建：

```cs
// Building candles from tick trades
subscription.MarketData.BuildFrom = DataType.Ticks;

// Building candles from order books
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// Building candles from Level1
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **K线构建字段** - 某些数据类型必须指定此参数：

```cs
// Building candles from the best bid price in Level1
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// Building candles from the best ask price in Level1
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// Building candles from the middle of the spread in the order book
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **成交量分布** - 计算K线的成交量分布：

```cs
// Enable volume profile calculation
subscription.MarketData.IsCalcVolumeProfile = true;
```

## 不同K线类型的订阅示例

### 标准时间周期K线

```cs
// 5-minute candles
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### 仅加载历史K线

```cs
// Loading only historical candles without transitioning to real-time
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // Specify end date
		BuildMode = MarketDataBuildModes.Load  // Only load ready-made data
	}
};
_connector.Subscribe(historicalSubscription);
```

### 使用逐笔成交构建非标准时间周期K线

```cs
// Candles with a 21-second timeframe, built from ticks
var customTimeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromSeconds(21)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(customTimeFrameSubscription);
```

### 使用市场深度数据构建K线

```cs
// Candles built from the middle of the spread in the order book
var depthBasedSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(1)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.MarketDepth,
		BuildField = Level1Fields.SpreadMiddle
	}
};
_connector.Subscribe(depthBasedSubscription);
```

### 带成交量分布的K线

```cs
// 5-minute candles with volume profile calculation
var volumeProfileSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.LoadAndBuild,
		BuildFrom = DataType.Ticks,
		IsCalcVolumeProfile = true
	}
};
_connector.Subscribe(volumeProfileSubscription);
```

### 成交量K线

```cs
// Volume candles (each candle contains 1000 contracts in volume)
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // Specify candle type and volume
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(volumeCandleSubscription);
```

### 成交笔数K线

```cs
// Tick count candles (each candle contains 1000 trades)
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // Specify candle type and number of trades
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(tickCandleSubscription);
```

### 价格范围K线

```cs
// Price range candles with a range of 0.1 units
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // Specify candle type and price range
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(rangeCandleSubscription);
```

### 砖形图K线

```cs
// Renko candles with a step of 0.1
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // Specify candle type and block size
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(renkoCandleSubscription);
```

### 点数图K线（P&F）

```cs
// Point and Figure candles
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // Specify P&F parameters
	security)
{
	MarketData =
	{
		BuildMode = MarketDataBuildModes.Build,
		BuildFrom = DataType.Ticks
	}
};
_connector.Subscribe(pnfCandleSubscription);
```

## 后续步骤

[图表](candles/chart.md)

[自定义K线类型](candles/custom_type_of_candle.md)
