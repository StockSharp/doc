# Working with Charts in Strategies

In StockSharp, the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class provides a convenient interface for visualizing trading activity on a chart. In this article, we'll look at how to access a chart from a strategy, create areas (ChartArea), add various elements (candles, indicators, trades), and render data.

## Accessing the Chart

### GetChart Method

To access the chart from a strategy, use the [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) method:

```cs
protected override void OnStarted(DateTimeOffset time)
{
	base.OnStarted(time);
	
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

The [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) method returns an [IChart](xref:StockSharp.Charting.IChart) interface that provides access to chart functions. It's important to check the result for `null`, as the chart may be unavailable, for example, when running a strategy in console mode or cloud testing.

### SetChart Method

In some cases, the chart may be set from outside. For this, use the [Strategy.SetChart](xref:StockSharp.Algo.Strategies.Strategy.SetChart(StockSharp.Charting.IChart)) method:

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

## Creating Chart Areas

After obtaining access to the chart, you can create one or more areas to display various data. Use the [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) method:

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

You can also use the [IChart.AddArea](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddArea(StockSharp.Charting.IChart)) method directly:

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

## Adding Elements to the Chart

After creating chart areas, you can add various elements to display data. StockSharp supports different types of elements such as candles, indicators, trades, and orders.

### Adding Candles

To display candles, use the [AddCandles](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddCandles(StockSharp.Charting.IChartArea)) method of the chart area:

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

The [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) interface provides many properties for configuring candle display:

- **DrawStyle** - candle display style:
  - **CandleStick** - Japanese candles
  - **Ohlc** - bars
  - **LineOpen/LineHigh/LineLow/LineClose** - lines for respective prices
  - **BoxVolume** - volume boxes
  - **ClusterProfile** - cluster profile
  - **Area** - area
  - **PnF** - point and figure chart

- **Color settings**:
  - **UpFillColor/DownFillColor** - rising/falling candle body color
  - **UpBorderColor/DownBorderColor** - rising/falling candle border color
  - **LineColor** - line color for line-type charts
  - **AreaColor** - area color for Area type

- **Other settings**:
  - **StrokeThickness** - line thickness
  - **AntiAliasing** - smoothing
  - **ShowAxisMarker** - show Y-axis marker

### Adding Indicators

To display indicators, use the [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) method:

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

The [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) method automatically creates an indicator element and adds it to the specified chart area. You can specify a color and additional color for display.

You can also add an indicator element directly through the chart area's [AddIndicator](xref:StockSharp.Charting.ChartingInterfacesExtensions.AddIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator)) method:

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

The [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) interface provides the following properties for configuration:

- **Color** - main indicator color
- **AdditionalColor** - additional color (for indicators with two lines)
- **StrokeThickness** - line thickness
- **AntiAliasing** - smoothing
- **DrawStyle** - drawing style (line, points, histogram, etc.)
- **ShowAxisMarker** - show Y-axis marker
- **AutoAssignYAxis** - automatically assign Y-axis

### Adding Trades

To display trades, use the [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) method:

```cs
// Adding an element to display trades
_tradesElement = DrawOwnTrades(_mainArea);

// Configuring trade display
_tradesElement.BuyBrush = Color.Green;  // Buy color
_tradesElement.SellBrush = Color.Red;   // Sell color
_tradesElement.PointSize = 10;          // Point size
```

### Adding Orders

To display orders, use the [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) method:

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

The [IChartOrderElement](xref:StockSharp.Charting.IChartOrderElement) interface provides the following properties for configuration:

- **ActiveBrush** - color of active orders
- **CanceledBrush** - color of canceled orders
- **DoneBrush** - color of completed orders
- **ErrorColor** - error color
- **ErrorStrokeColor** - error border color
- **Filter** - order display filter

## Drawing Data on the Chart

After configuring all chart elements, you can proceed to draw data. Different methods are used depending on the type of data.

### Drawing Candles and Indicators

The most efficient way to draw data is to use the [IChart.Draw](xref:StockSharp.Charting.IThemeableChart.Draw(StockSharp.Charting.IChartDrawData)) method with an [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) object:

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

The [IChart.CreateData](xref:StockSharp.Charting.IThemeableChart.CreateData) method creates an [IChartDrawData](xref:StockSharp.Charting.IChartDrawData) object used to group and add data for different chart elements. Data grouping is done by timestamp using the [Group](xref:StockSharp.Charting.IChartDrawData.Group(System.DateTimeOffset)) method.

For adding data of different types, various overloads of the [Add](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem.Add(StockSharp.Charting.IChartCandleElement,StockSharp.Messages.DataType,StockSharp.Messages.SecurityId,System.Decimal,System.Decimal,System.Decimal,System.Decimal,StockSharp.Messages.CandlePriceLevel[],StockSharp.Messages.CandleStates)) method of the [IChartDrawDataItem](xref:StockSharp.Charting.IChartDrawData.IChartDrawDataItem) object are used.

### Drawing Trades and Orders

For drawing trades and orders, an automatic mechanism is usually used that is triggered when new trades are received or orders change. However, if manual drawing is required, you can use the following code:

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

## Full Example of Chart Rendering in a Strategy

Below is a complete example of a strategy with chart setup and rendering:

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
	
	protected override void OnStarted(DateTimeOffset time)
	{
		base.OnStarted(time);
		
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

## Conclusion

Using charts in StockSharp strategies allows visualizing trading activity, which significantly simplifies the development, debugging, and monitoring of trading strategies. The [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class provides many methods for working with charts, allowing easy addition of various elements and rendering data.

When developing a strategy with a graphical interface, always consider that the chart may be unavailable, for example, when running in console mode or cloud testing. Therefore, it's important to check the result of the [GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) method for `null` and provide an alternative scenario for the strategy to work without visualization.

## See Also

- [Indicators in Strategy](indicators.md)
- [Trading Operations in Strategies](trading_operations.md)