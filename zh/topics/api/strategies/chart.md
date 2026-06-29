# 在策略中使用图表

在 StockSharp 中，[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类提供了一个便捷的界面，用于在图表上可视化交易活动。在本文中，我们将探讨如何从策略访问图表、创建区域 (ChartArea)、添加各种元素（蜡烛图、指标、交易）以及渲染数据。

## 访问图表

### 获取图表方法

要从策略中访问图表，请使用 [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) 方法：

```cs
protected override void OnStarted2(DateTime time)
{
	base.OnStarted2(time);
	
	// Obtaining the chart
	_chart = GetChart();
	
	// Checking chart availability
	if (_chart != null)
	{
		// Initializing the chart
		InitializeChart();
	}
	else
	{
		// Chart is unavailable, for example, when running in console mode
		LogInfo("Chart is unavailable. Visualization disabled.");
	}
}
```

[GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) 方法返回一个 [IChart](xref:StockSharp.Charting.IChart) 接口，该接口提供对图表功能的访问。检查结果是否为 `null` 非常重要，因为图表可能不可用，例如，在控制台模式或云端测试中运行策略时。

### SetChart 方法

在某些情况下，图表可能会从外部设置。为此，请使用 [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)) 方法：

```cs
// Setting the chart from an external source
public void ConfigureVisualization(IChart chart)
{
	SetChart(chart);
	
	if (chart != null)
	{
		InitializeChart();
	}
}
```

## 创建图表区域

在获取图表访问权限后，您可以创建一个或多个区域来显示各种数据。使用 [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) 方法：

```cs
private void InitializeChart()
{
	// Creating the main area for candles and indicators
	_mainArea = CreateChartArea();
	
	// Creating an additional area for volume
	_volumeArea = CreateChartArea();
	
	// Configuring areas and adding elements
	ConfigureChartElements();
}
```

你也可以直接使用 [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart) 方法：

```cs
private void InitializeChart()
{
	// Clear existing areas if necessary
	foreach (var area in _chart.Areas.ToArray())
		_chart.RemoveArea(area);
	
	// Create the main area for candles and indicators
	_mainArea = _chart.AddArea();
	
	// Create an additional area for volume
	_volumeArea = _chart.AddArea();
	
	// Configure areas and add elements
	ConfigureChartElements();
}
```

## 向图表添加元素

创建图表区域后，您可以添加各种元素来显示数据。StockSharp 支持不同类型的元素，如蜡烛图、指标、交易和订单。

### 添加蜡烛

要显示蜡烛图，请使用图表区域的 [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) 方法：

```cs
private void ConfigureChartElements()
{
	// Adding a candle element to the main area
	_candleElement = _mainArea.AddCandles();
	
	// Configuring candle display
	_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick; // Japanese candles
	_candleElement.AntiAliasing = true; // Smoothing
	_candleElement.UpFillColor = Color.Green; // Rising candle body color
	_candleElement.DownFillColor = Color.Red; // Falling candle body color
	_candleElement.UpBorderColor = Color.DarkGreen; // Rising candle border color
	_candleElement.DownBorderColor = Color.DarkRed; // Falling candle border color
	_candleElement.StrokeThickness = 1; // Line thickness
	_candleElement.ShowAxisMarker = true; // Show Y-axis marker
}
```

[IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) 接口提供了许多用于配置蜡烛显示的属性：

- **绘图样式** - 蜡烛图显示风格：
  - **蜡烛图** - 日本蜡烛
  - **OHLC** - 柱状图
  - **LineOpen/LineHigh/LineLow/LineClose** - 分别表示相应价格的线
  - **箱体体积** - 体积盒
  - **ClusterProfile** - 集群配置文件
  - **区域** - 区域
  - **PnF** - 点数图表

- **颜色设置**：
  - **UpFillColor/DownFillColor** - 上涨/下跌蜡烛实体颜色
  - **UpBorderColor/DownBorderColor** - 上涨/下跌蜡烛边框颜色
  - **LineColor** - 线型图的线条颜色
  - **AreaColor** - 区域类型的区域颜色

- **其他设置**：
  - **StrokeThickness** - 线条粗细
  - **抗锯齿** - 平滑
  - **显示坐标轴标记** - 显示 Y 轴标记

### 添加指标

要显示指标，请使用 [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) 方法：

```cs
// Creating indicators
_sma = new SimpleMovingAverage { Length = SmaLength };
_bollinger = new BollingerBands
{
	Length = BollingerLength,
	Deviation = BollingerDeviation
};

// Adding indicators to the strategy collection
Indicators.Add(_sma);
Indicators.Add(_bollinger);

// Visualizing indicators
_smaElement = DrawIndicator(_mainArea, _sma, Color.Blue);
_bollingerUpperElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerLowerElement = DrawIndicator(_mainArea, _bollinger, Color.Purple);
_bollingerMiddleElement = DrawIndicator(_mainArea, _bollinger, Color.Gray);
```

[DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) 方法会自动创建一个指示器元素并将其添加到指定的图表区域。您可以指定显示的颜色和附加颜色。

您也可以通过图表区域的 [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) 方法直接添加指示器元素：

```cs
// Adding SMA directly through the chart area
var smaElement = _mainArea.AddIndicator(_sma);
smaElement.Color = Color.Blue;
smaElement.StrokeThickness = 2;
smaElement.DrawStyle = DrawStyles.Line;
smaElement.AntiAliasing = true;
smaElement.ShowAxisMarker = true;
smaElement.AutoAssignYAxis = true; // Automatically assign Y-axis
```

[IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) 接口提供以下属性用于配置：

- **颜色** - 主要指示颜色
- **附加颜色** - 附加颜色（用于有两条线的指示器）
- **StrokeThickness** - 线条粗细
- **抗锯齿** - 平滑
- **DrawStyle** - 绘图风格（线条、点、直方图等）
- **显示坐标轴标记** - 显示 Y 轴标记
- **AutoAssignYAxis** - 自动分配 Y 轴

### 添加交易

要显示交易，请使用 [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea) 方法：

```cs
// Adding an element to display trades
_tradesElement = DrawOwnTrades(_mainArea);

// Configuring trade display
_tradesElement.BuyBrush = Color.Green;  // Buy color
_tradesElement.SellBrush = Color.Red;   // Sell color
_tradesElement.PointSize = 10;          // Point size
```

### 添加订单

要显示订单，请使用 [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) 方法：

```cs
// Adding an element to display orders
_ordersElement = DrawOrders(_mainArea);

// Configuring order display
_ordersElement.ActiveBrush = Color.Blue;     // Active orders color
_ordersElement.CanceledBrush = Color.Gray;   // Canceled orders color
_ordersElement.DoneBrush = Color.Green;      // Completed orders color
_ordersElement.ErrorColor = Color.Red;       // Error color
_ordersElement.PointSize = 8;                // Point size
```

[IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) 接口提供以下属性用于配置：

- **ActiveBrush** - 活动订单的颜色
- **已取消画笔** - 已取消订单的颜色
- **已完成笔刷** - 已完成订单的颜色
- **错误颜色** - 错误颜色
- **ErrorStrokeColor** - 错误边框颜色
- **筛选** - 订单显示筛选

## 在图表上绘制数据

在配置所有图表元素后，您可以开始绘制数据。根据数据的类型使用不同的方法。

### 绘制蜡烛图和指标

绘制数据的最高效方法是使用 [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData) 方法配合 [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) 对象：

```cs
private void ProcessCandle(ICandleMessage candle)
{
	// Processing candle in indicators
	var smaValue = _sma.Process(candle);
	var bollingerValue = _bollinger.Process(candle);
	
	// If chart is unavailable, skip drawing
	if (_chart == null)
		return;
	
	// Create data for drawing
	var drawData = _chart.CreateData();
	
	// Group data by candle time
	var group = drawData.Group(candle.OpenTime);
	
	// Add candle
	group.Add(_candleElement, 
		candle.DataType, 
		candle.SecurityId, 
		candle.OpenPrice, 
		candle.HighPrice, 
		candle.LowPrice, 
		candle.ClosePrice, 
		candle.PriceLevels, 
		candle.State);
	
	// Add indicator values
	group.Add(_smaElement, smaValue);
	
	if (bollingerValue != null)
	{
		group.Add(_bollingerUpperElement, bollingerValue);
		group.Add(_bollingerMiddleElement, bollingerValue);
		group.Add(_bollingerLowerElement, bollingerValue);
	}
	
	// Draw data on the chart
	_chart.Draw(drawData);
}
```

[IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) 方法创建一个 [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) 对象，用于分组和添加不同图表元素的数据。数据分组是通过时间戳使用 [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset) 方法完成的。

对于添加不同类型的数据，使用 [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem) 对象的 [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates) 方法的各种重载。

### 绘制交易和订单

对于交易和订单的绘制，通常使用一种自动机制，当接收到新交易或订单发生变化时触发。然而，如果需要手动绘制，可以使用以下代码：

```cs
// Drawing a trade
var tradeDrawData = _chart.CreateData();
var tradeGroup = tradeDrawData.Group(trade.Time);
tradeGroup.Add(_tradesElement, trade.Id, trade.StringId, trade.Side, trade.Price, trade.Volume);
_chart.Draw(tradeDrawData);

// Drawing an order
var orderDrawData = _chart.CreateData();
var orderGroup = orderDrawData.Group(order.Time);
orderGroup.Add(_ordersElement, order.Id, order.StringId, order.Side, order.Price, order.Volume);
_chart.Draw(orderDrawData);
```

## 策略中图表渲染的完整示例

下面是一个包含图表设置和渲染的完整策略示例：

```cs
public class SmaStrategy : Strategy
{
	private readonly StrategyParam<int> _smaLength;
	private readonly StrategyParam<int> _bollingerLength;
	private readonly StrategyParam<decimal> _bollingerDeviation;
	
	private SimpleMovingAverage _sma;
	private BollingerBands _bollinger;
	
	private IChart _chart;
	private IChartArea _mainArea;
	private IChartArea _volumeArea;
	
	private IChartCandleElement _candleElement;
	private IChartIndicatorElement _smaElement;
	private IChartIndicatorElement _bollingerUpperElement;
	private IChartIndicatorElement _bollingerMiddleElement;
	private IChartIndicatorElement _bollingerLowerElement;
	private IChartOrderElement _ordersElement;
	private IChartTradeElement _tradesElement;
	
	public SmaStrategy()
	{
		_smaLength = Param(nameof(SmaLength), 20);
		_bollingerLength = Param(nameof(BollingerLength), 20);
		_bollingerDeviation = Param(nameof(BollingerDeviation), 2m);
	}
	
	public int SmaLength
	{
		get => _smaLength.Value;
		set => _smaLength.Value = value;
	}
	
	public int BollingerLength
	{
		get => _bollingerLength.Value;
		set => _bollingerLength.Value = value;
	}
	
	public decimal BollingerDeviation
	{
		get => _bollingerDeviation.Value;
		set => _bollingerDeviation.Value = value;
	}
	
	protected override void OnStarted2(DateTime time)
	{
		base.OnStarted2(time);
		
		// Creating indicators
		_sma = new SimpleMovingAverage { Length = SmaLength };
		_bollinger = new BollingerBands
		{
			Length = BollingerLength,
			Deviation = BollingerDeviation
		};
		
		// Adding indicators to strategy collection
		Indicators.Add(_sma);
		Indicators.Add(_bollinger);
		
		// Getting the chart
		_chart = GetChart();
		
		// Initializing the chart if available
		if (_chart != null)
		{
			InitializeChart();
		}
		
		// Subscribing to candles
		var subscription = new Subscription(
			DataType.TimeFrame(TimeSpan.FromMinutes(5)),
			Security);
		
		subscription
			.WhenCandlesFinished(this)
			.Do(ProcessCandle)
			.Apply(this);
		
		Subscribe(subscription);
	}
	
	private void InitializeChart()
	{
		// Clear existing areas
		foreach (var area in _chart.Areas.ToArray())
			_chart.RemoveArea(area);
		
		// Create the main area for candles and indicators
		_mainArea = _chart.AddArea();
		
		// Create an additional area for volume
		_volumeArea = _chart.AddArea();
		
		// Configure chart elements
		ConfigureChartElements();
	}
	
	private void ConfigureChartElements()
	{
		// Adding an element for displaying candles
		_candleElement = _mainArea.AddCandles();
		_candleElement.DrawStyle = ChartCandleDrawStyles.CandleStick;
		_candleElement.AntiAliasing = true;
		_candleElement.UpFillColor = Color.Green;
		_candleElement.DownFillColor = Color.Red;
		_candleElement.UpBorderColor = Color.DarkGreen;
		_candleElement.DownBorderColor = Color.DarkRed;
		_candleElement.StrokeThickness = 1;
		_candleElement.ShowAxisMarker = true;
		
		// Adding elements for indicators
		_smaElement = _mainArea.AddIndicator(_sma);
		_smaElement.Color = Color.Blue;
		_smaElement.StrokeThickness = 2;
		
		_bollingerUpperElement = _mainArea.AddIndicator(_bollinger);
		_bollingerUpperElement.Color = Color.Purple;
		_bollingerUpperElement.StrokeThickness = 1;
		
		_bollingerMiddleElement = _mainArea.AddIndicator(_bollinger);
		_bollingerMiddleElement.Color = Color.Gray;
		_bollingerMiddleElement.StrokeThickness = 1;
		
		_bollingerLowerElement = _mainArea.AddIndicator(_bollinger);
		_bollingerLowerElement.Color = Color.Purple;
		_bollingerLowerElement.StrokeThickness = 1;
		
		// Adding elements for orders and trades
		_ordersElement = DrawOrders(_mainArea);
		_tradesElement = DrawOwnTrades(_mainArea);
	}
	
	private void ProcessCandle(ICandleMessage candle)
	{
		// Processing candle with indicators
		var smaValue = _sma.Process(candle);
		var bollingerValue = _bollinger.Process(candle);
		
		// If chart is unavailable, skip drawing
		if (_chart == null)
			return;
		
		// Drawing data on the chart
		var drawData = _chart.CreateData();
		var group = drawData.Group(candle.OpenTime);
		
		// Adding candle
		group.Add(_candleElement, 
			candle.DataType, 
			candle.SecurityId, 
			candle.OpenPrice, 
			candle.HighPrice, 
			candle.LowPrice, 
			candle.ClosePrice, 
			candle.PriceLevels, 
			candle.State);
		
		// Adding indicator values
		group.Add(_smaElement, smaValue);
		
		if (bollingerValue != null)
		{
			group.Add(_bollingerUpperElement, bollingerValue);
			group.Add(_bollingerMiddleElement, bollingerValue);
			group.Add(_bollingerLowerElement, bollingerValue);
		}
		
		// Drawing data on the chart
		_chart.Draw(drawData);
		
		// Trading logic
		if (!IsFormed)
			return;
			
		// ... implementation of trading logic ...
	}
}
```

## 结论

在 StockSharp 策略中使用图表可以可视化交易活动，这显著简化了交易策略的开发、调试和监控。[Strategy](xref:StockSharp.Algo.Strategies.Strategy) 类提供了许多用于处理图表的方法，可以轻松添加各种元素并呈现数据。

在使用图形界面开发策略时，应始终考虑图表可能不可用，例如在控制台模式或云端测试时。因此，检查 [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) 方法的结果对于 `null` 非常重要，并为策略在无可视化的情况下运行提供替代方案。

## 另请参阅

- [战略中的指标](indicators.md)
- [策略中的交易操作](trading_operations.md)