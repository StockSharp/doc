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

![时间周期K线示例](../../images/sample_timeframecandles.png)

![范围K线示例](../../images/sample_rangecandles.png)

## 开始获取数据

1. 要获取K线，请使用 [Subscription](xref:StockSharp.BusinessEntities.Subscription) 类创建订阅：

```cs
// 创建 5 分钟 K线订阅
var subscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),  // 带有时间框架设置的数据类型
	security)  // 交易品种
{
	// 通过 MarketData 属性配置附加参数
	MarketData = 
	{
		// 请求历史数据的期间（最近 30 天）
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Now
	}
};
```

2. 要接收K线，请订阅 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件。出现可供处理的新值时，该事件会发出通知：

```cs
// 订阅 K线接收事件
_connector.CandleReceived += OnCandleReceived;

// K线接收事件处理器
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 这里 subscription 是我们创建的订阅对象
	// 接收到的 K线
	
	// 检查 K线是否属于我们的订阅
	if (subscription == _candleSubscription)
	{
		// 在图表上绘制 K线
		Chart.Draw(_candleElement, candle);
	}
}
```

> [!TIP]
> [Chart](xref:StockSharp.Xaml.Charting.Chart) 图形组件用于显示K线。

3. 接下来，通过 [Connector.Subscribe](xref:StockSharp.Algo.Connector.Subscribe(StockSharp.BusinessEntities.Subscription)) 方法启动订阅：

```cs
// 启动订阅
_connector.Subscribe(subscription);
```

之后会开始调用 [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件。

4. [Connector.CandleReceived](xref:StockSharp.Algo.Connector.CandleReceived) 事件不仅会在出现新K线时调用，当前K线发生变化时也会调用。

如果只需要显示**已完成**的K线，应检查所接收K线的 [ICandleMessage.State](xref:StockSharp.Messages.ICandleMessage.State) 属性：

```cs
private void OnCandleReceived(Subscription subscription, ICandleMessage candle)
{
	// 检查 K线是否属于我们的订阅
	if (subscription != _candleSubscription)
		return;
	
	// 检查 K线是否已完成
	if (candle.State == CandleStates.Finished) 
	{
		// 创建绘制数据
		var chartData = new ChartDrawData();
		chartData.Group(candle.OpenTime).Add(_candleElement, candle);
		
		// 在图表上绘制 K线
		this.GuiAsync(() => Chart.Draw(chartData));
	}
}
```

5. 还可以为订阅配置其他参数：

- **K线构建模式** - 指定请求现成数据，还是使用其他数据类型构建K线：

```cs
// 仅请求现成数据
subscription.MarketData.BuildMode = MarketDataBuildModes.Load;

// 仅从其他数据类型构建
subscription.MarketData.BuildMode = MarketDataBuildModes.Build;

// 请求现成数据，若不可用则构建
subscription.MarketData.BuildMode = MarketDataBuildModes.LoadAndBuild;
```

- **K线构建数据源** - 如果无法直接获取K线，指定使用哪种数据类型进行构建：

```cs
// 从 tick 成交构建 K线
subscription.MarketData.BuildFrom = DataType.Ticks;

// 从订单簿构建 K线
subscription.MarketData.BuildFrom = DataType.MarketDepth;

// 从 Level1 构建 K线
subscription.MarketData.BuildFrom = DataType.Level1;
```

- **K线构建字段** - 某些数据类型必须指定此参数：

```cs
// 按 Level1 中的最佳 bid 价格构建 K线
subscription.MarketData.BuildField = Level1Fields.BestBidPrice;

// 按 Level1 中的最佳 ask 价格构建 K线
subscription.MarketData.BuildField = Level1Fields.BestAskPrice;

// 按订单簿中的点差中间价构建 K线
subscription.MarketData.BuildField = Level1Fields.SpreadMiddle;
```

- **成交量分布** - 计算K线的成交量分布：

```cs
// 启用成交量分布计算
subscription.MarketData.IsCalcVolumeProfile = true;
```

## 不同K线类型的订阅示例

### 标准时间周期K线

```cs
// 5 分钟 K线
var timeFrameSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security);
_connector.Subscribe(timeFrameSubscription);
```

### 仅加载历史K线

```cs
// 仅加载历史 K线，不切换到实时模式
var historicalSubscription = new Subscription(
	DataType.TimeFrame(TimeSpan.FromMinutes(5)),
	security)
{
	MarketData =
	{
		From = DateTime.Today.Subtract(TimeSpan.FromDays(30)),
		To = DateTime.Today,  // 指定结束日期
		BuildMode = MarketDataBuildModes.Load  // 仅加载现成数据
	}
};
_connector.Subscribe(historicalSubscription);
```

### 使用逐笔成交构建非标准时间周期K线

```cs
// 从 tick 构建的 21 秒周期 K线
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
// 按订单簿点差中间价构建的 K线
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
// 带成交量分布计算的 5 分钟 K线
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
// 成交量 K线（每根 K线包含 1000 份合约的成交量）
var volumeCandleSubscription = new Subscription(
	DataType.Volume(1000m),  // 指定 K线类型和成交量
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
// tick 数 K线（每根 K线包含 1000 笔成交）
var tickCandleSubscription = new Subscription(
	DataType.Tick(1000),  // 指定 K线类型和成交笔数
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
// 范围为 0.1 单位的价格区间 K线
var rangeCandleSubscription = new Subscription(
	DataType.Range(0.1m),  // 指定 K线类型和价格范围
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
// 步长为 0.1 的 Renko K线
var renkoCandleSubscription = new Subscription(
	DataType.Renko(0.1m),  // 指定 K线类型和砖块大小
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
// 点数图K线
var pnfCandleSubscription = new Subscription(
	DataType.PnF(new PnfArg { BoxSize = 0.1m, ReversalAmount = 1 }),  // 指定 P&F 参数
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
