# High-Level API in Strategies

StockSharp provides a set of high-level APIs to simplify working with common tasks in trading strategies. These interfaces allow writing cleaner code that focuses on trading logic rather than technical details.

## Simplified Subscription Management

High-level methods for working with subscriptions hide the complexities of managing subscription lifecycle and data processing.

### SubscribeCandles Method

Instead of manually creating a subscription and setting up event handlers, you can use the [SubscribeCandles](xref:StockSharp.Algo.Strategies.Strategy.SubscribeCandles(System.TimeSpan,System.Boolean,StockSharp.BusinessEntities.Security)) method:

```cs
// Create and configure a candle subscription in a single line
var subscription = SubscribeCandles(CandleType);
```

This method returns an object of type [ISubscriptionHandler\<ICandleMessage\>](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1), which provides a convenient interface for further subscription configuration.

### Automatic Binding of Indicators with Subscription

The high-level API makes it easy to bind indicators to a data subscription:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
	// Bind indicators to candle subscription
	.Bind(longSma, shortSma, OnProcess)
	// Start processing
	.Start();
```

#### Automatic Addition of Indicators to Strategy.Indicators Collection

It's important to note that when using the [Bind](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.Bind(StockSharp.Algo.Indicators.IIndicator,StockSharp.Algo.Indicators.IIndicator,System.Action{`0,System.Decimal,System.Decimal})) method to link indicators with a subscription, you **do not need** to additionally add these indicators to the [Strategy.Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection, as is typically done in the traditional approach (described in the [indicators documentation](indicators.md)). The system automatically:

1. Adds indicators to the [Indicators](xref:StockSharp.Algo.Strategies.Strategy.Indicators) collection
2. Tracks the formation state of indicators
3. Updates the strategy's [IsFormed](xref:StockSharp.Algo.Strategies.Strategy.IsFormed) state

This significantly simplifies code and reduces the likelihood of errors.

If it is necessary to receive indicator values even when some of them have no
data yet (`IIndicatorValue.IsEmpty` is `true`), use the
`BindWithEmpty` method. In this case the handler arguments must be of type
`decimal?`. You can also use `BindEx` to inspect the raw
`IIndicatorValue` objects directly.

#### Using BindEx to Work with Raw Indicator Values

If an indicator returns non-standard values (not just numbers), you can use the [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) method, which provides access to the original [IIndicatorValue](xref:StockSharp.Algo.Indicators.IIndicatorValue) object:

```cs
subscription
	.BindEx(indicator, OnProcessWithRawValue)
	.Start();

// Handler receives the original IIndicatorValue
private void OnProcessWithRawValue(ICandleMessage candle, IIndicatorValue value)
{
	// Access to IIndicatorValue properties
	if (value.IsFinal)
	{
		// For indicators returning boolean values
		var boolValue = value.GetValue<bool>();
		
		// Or other data types specific to a particular indicator
		// ...
	}
}
```

The [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) method is particularly useful in the following cases:

- Working with indicators that return boolean values (e.g., [Fractals](xref:StockSharp.Algo.Indicators.Fractals))
- Accessing additional properties of the indicator value type (e.g., the [IsFinal](xref:StockSharp.Algo.Indicators.IIndicatorValue.IsFinal) flag)
- Working with indicators that return structured data

#### Working with Complex Indicators (IComplexIndicator)

For complex indicators that contain multiple internal indicators (e.g., [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands), [MACD](xref:StockSharp.Algo.Indicators.MovingAverageConvergenceDivergence)), the API provides special overloads of the `Bind` and `BindEx` methods:

```cs
// Create a complex indicator
var bollinger = new BollingerBands 
{ 
	Length = 20, 
	Deviation = 2 
};

// Bind the complex indicator to a subscription
subscription
	.BindEx(bollinger, OnProcessBollinger)
	.Start();

// Handler receives the BollingerBandsValue instance
private void OnProcessBollinger(ICandleMessage candle, IIndicatorValue value)
{
	var typed = (BollingerBandsValue)value;

	// Use Bollinger band values
	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
}
```

For more flexible work, you can use [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) with direct access to the complex indicator value:

```cs
subscription.BindEx(bollinger, (candle, indicatorValue) =>
{
	var typed = (BollingerBandsValue)indicatorValue;

	if (candle.ClosePrice >= typed.UpBand && Position >= 0)
		SellMarket(Volume + Math.Abs(Position));
	else if (candle.ClosePrice <= typed.LowBand && Position <= 0)
		BuyMarket(Volume + Math.Abs(Position));
});
```

The [BindEx](xref:StockSharp.Algo.Strategies.ISubscriptionHandler`1.BindEx(StockSharp.Algo.Indicators.IIndicator,System.Action{`0,StockSharp.Algo.Indicators.IIndicatorValue},System.Boolean)) method for complex indicators automatically:

1. Processes input data through the complex indicator
2. Passes the resulting `IIndicatorValue` to the specified handler

Cast the value to the indicator's dedicated **value type** to work with its individual fields.

### The `Bind` Method establishes a connection between subscription data and indicators. When a new candle is received:

1. The candle is automatically sent for processing to the indicators
2. Processing results are passed to the specified handler (in the example, the `OnProcess` method)
3. All synchronization and state management code is hidden from the developer

The handler receives ready-to-use values as simple `decimal` types. The method is called only when all bound indicators return data:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
	// Work directly with ready-made indicator values
	var isShortLessThenLong = shortValue < longValue;
	
	// Trading logic uses clean numeric values
	// without the need to extract them from IIndicatorValue
	// ...
}
```

This significantly simplifies code and makes it more readable, as the developer doesn't need to:
- Manually handle the event of receiving a candle
- Pass data to indicators manually
- Extract values from indicator results

## Simplified Chart Management

### Automatic Visualization

The high-level API provides simple methods for binding subscriptions and indicators to chart elements:

```cs
var area = CreateChartArea();

// area can be null when running without GUI
if (area != null)
{
	// Automatic binding of candles to chart area
	DrawCandles(area, subscription);

	// Drawing indicators with color customization
	DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
	DrawIndicator(area, longSma);
	
	// Drawing own trades
	DrawOwnTrades(area);
	
	// Drawing orders
	DrawOrders(area);
}
```

#### DrawCandles Method

The [DrawCandles](xref:StockSharp.Algo.Strategies.Strategy.DrawCandles(StockSharp.Charting.IChartArea,StockSharp.BusinessEntities.Subscription)) method automatically links a candle subscription to a chart candle display element:

```cs
// Create a chart element for displaying candles
IChartCandleElement candles = DrawCandles(area, subscription);

// Additional element parameters can be configured
candles.DrawOpenClose = true;  // Display open/close lines
candles.DrawHigh = true;       // Display highs
candles.DrawLow = true;        // Display lows
```

The method returns a [IChartCandleElement](xref:StockSharp.Charting.IChartCandleElement) chart element that can be further customized.

#### DrawIndicator Method

The [DrawIndicator](xref:StockSharp.Algo.Strategies.Strategy.DrawIndicator(StockSharp.Charting.IChartArea,StockSharp.Algo.Indicators.IIndicator,System.Nullable{System.Drawing.Color},System.Nullable{System.Drawing.Color})) method creates and configures a chart element for displaying indicator values:

```cs
// Simple addition of an indicator to the chart with default color
IChartIndicatorElement smaElem = DrawIndicator(area, sma);

// Adding an indicator with a specified primary color
IChartIndicatorElement rsiFast = DrawIndicator(area, rsi, System.Drawing.Color.Red);

// Adding an indicator with specified primary and secondary colors
IChartIndicatorElement bollingerElem = DrawIndicator(
	area, 
	bollinger, 
	System.Drawing.Color.Blue,    // Primary color
	System.Drawing.Color.Gray     // Secondary color (for the second line)
);

// Additional element configuration
smaElem.DrawStyle = DrawStyles.Line;           // Drawing style: line
rsiFast.DrawStyle = DrawStyles.Dot;            // Drawing style: dots
bollingerElem.DrawStyle = DrawStyles.Dashdot;  // Drawing style: dash-dot
```

The method returns a [IChartIndicatorElement](xref:StockSharp.Charting.IChartIndicatorElement) chart element that can be customized. For indicators with multiple values (e.g., [BollingerBands](xref:StockSharp.Algo.Indicators.BollingerBands)), the primary color is applied to the first value, and the secondary color to the second.

#### DrawOwnTrades Method

The [DrawOwnTrades](xref:StockSharp.Algo.Strategies.Strategy.DrawOwnTrades(StockSharp.Charting.IChartArea)) method creates an element for displaying strategy's own trades on the chart:

```cs
// Create an element for displaying trades
IChartTradeElement trades = DrawOwnTrades(area);

// Element configuration
trades.BuyColor = System.Drawing.Color.Green;   // Color for buy trades
trades.SellColor = System.Drawing.Color.Red;    // Color for sell trades
trades.FullTitle = "My Strategy Trades";        // Element title
```

This method automatically sets up the display of all trades executed by the strategy. Trades are displayed on the chart as markers at the points where they were executed, taking into account the trade side (buy/sell).

#### DrawOrders Method

The [DrawOrders](xref:StockSharp.Algo.Strategies.Strategy.DrawOrders(StockSharp.Charting.IChartArea)) method creates an element for displaying orders on the chart:

```cs
// Create an element for displaying orders
IChartOrderElement orders = DrawOrders(area);

// Element configuration
orders.BuyPendingColor = System.Drawing.Color.DarkGreen;   // Color for active buy orders
orders.SellPendingColor = System.Drawing.Color.DarkRed;    // Color for active sell orders
orders.BuyColor = System.Drawing.Color.Green;              // Color for executed buy orders
orders.SellColor = System.Drawing.Color.Red;               // Color for executed sell orders
orders.CancelColor = System.Drawing.Color.Gray;            // Color for canceled orders
```

This method automatically sets up the display of all orders placed by the strategy. Orders are displayed as markers at their price levels with different color coding for different order states.

#### CreateChartArea Method

The [CreateChartArea](xref:StockSharp.Algo.Strategies.Strategy.CreateChartArea) method creates a new area on the strategy chart:

```cs
// Create the first area for candles and indicators
var mainArea = CreateChartArea();
DrawCandles(mainArea, subscription);
DrawIndicator(mainArea, sma);

// Create a second area for separate indicators (e.g., RSI)
var secondArea = CreateChartArea();
DrawIndicator(secondArea, rsi);
```

Dividing the chart into areas allows for a more visual display of different data types. For example, indicators with a value range different from price (RSI, stochastic, etc.) are better displayed in separate areas.

Advantages of high-level visualization methods:
- No need to manually create `ChartDrawData` objects
- No need to manage data grouping by time
- No need to call `chart.Draw()` to update the chart
- Automatic data synchronization between subscriptions and chart elements
- Simplified management of graphical element appearance

The system automatically updates the chart when new data is received, allowing the developer to avoid focusing on technical visualization details.

## Position Protection

### StartProtection Method

To protect open positions, StockSharp provides the high-level [StartProtection](xref:StockSharp.Algo.Strategies.Strategy.StartProtection(StockSharp.Messages.Unit,StockSharp.Messages.Unit,System.Boolean,System.Nullable{System.TimeSpan},System.Nullable{System.TimeSpan},System.Boolean)) method:

```cs
// Start position protection with Take Profit and Stop Loss levels
StartProtection(TakeValue, StopValue);
```

This method automatically sets up protection for all open positions:
- Tracks price changes
- Automatically creates orders to close positions when Take Profit or Stop Loss levels are reached
- Supports various types of measurement units (absolute values, percentages, points)
- Can use trailing stop for adaptive position protection

Example with additional parameters:

```cs
// Start protection with trailing stop and market orders
StartProtection(
	takeProfit: new Unit(50, UnitTypes.Absolute), // Take Profit
	stopLoss: new Unit(2, UnitTypes.Percent),     // Stop Loss in percentage
	isStopTrailing: true,                         // Enable trailing stop
	useMarketOrders: true                         // Use market orders
);
```

## Advantages of High-Level API

The high-level API in StockSharp strategies provides the following advantages:

1. **Code Volume Reduction** - performing common tasks requires fewer lines of code

2. **Separation of Responsibilities** - trading logic is separated from technical details of data processing and visualization

3. **Improved Readability** - code becomes more understandable and expressive, focused on business logic

4. **Reduced Error Probability** - many typical errors are eliminated through automation of routine tasks

5. **Working with Clean Data Types** - instead of working with complex objects, you can operate with simple data types (e.g., `decimal`)

## Example Strategy Using High-Level API

Below is a complete example of a strategy demonstrating the use of the high-level API:

```cs
public class SmaStrategy : Strategy
{
	private bool? _isShortLessThenLong;

	public SmaStrategy()
	{
		_candleType = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
		_long = Param(nameof(Long), 80);
		_short = Param(nameof(Short), 30);
		_takeValue = Param(nameof(TakeValue), new Unit(50, UnitTypes.Absolute));
		_stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
	}

	private readonly StrategyParam<DataType> _candleType;
	public DataType CandleType
	{
		get => _candleType.Value;
		set => _candleType.Value = value;
	}

	private readonly StrategyParam<int> _long;
	public int Long
	{
		get => _long.Value;
		set => _long.Value = value;
	}

	private readonly StrategyParam<int> _short;
	public int Short
	{
		get => _short.Value;
		set => _short.Value = value;
	}

	private readonly StrategyParam<Unit> _takeValue;
	public Unit TakeValue
	{
		get => _takeValue.Value;
		set => _takeValue.Value = value;
	}

	private readonly StrategyParam<Unit> _stopValue;
	public Unit StopValue
	{
		get => _stopValue.Value;
		set => _stopValue.Value = value;
	}

	protected override void OnStarted(DateTimeOffset time)
	{
		base.OnStarted(time);

		// Create indicators
		var longSma = new SMA { Length = Long };
		var shortSma = new SMA { Length = Short };

		// Create a candle subscription and bind to indicators
		var subscription = SubscribeCandles(CandleType);
		subscription
			.Bind(longSma, shortSma, OnProcess)
			.Start();

		// Configure visualization
		var area = CreateChartArea();
		if (area != null)
		{
			DrawCandles(area, subscription);
			DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
			DrawIndicator(area, longSma);
			DrawOwnTrades(area);
		}

		// Start position protection
		StartProtection(TakeValue, StopValue);
	}

	private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
	{
		// Process only finished candles
		if (candle.State != CandleStates.Finished)
			return;

		// Trading logic based on indicator crossover
		var isShortLessThenLong = shortValue < longValue;

		if (_isShortLessThenLong == null)
		{
			_isShortLessThenLong = isShortLessThenLong;
		}
		else if (_isShortLessThenLong != isShortLessThenLong)
		{
			// Crossover occurred
			var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
			var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
			var priceStep = GetSecurity().PriceStep ?? 1;
			var price = candle.ClosePrice + (direction == Sides.Buy ? priceStep : -priceStep);

			// Place an order
			if (direction == Sides.Buy)
				BuyLimit(price, volume);
			else
				SellLimit(price, volume);

			// Save current indicator position
			_isShortLessThenLong = isShortLessThenLong;
		}
	}
}
```

## Conclusion

The high-level API in StockSharp significantly simplifies the development of trading strategies, allowing developers to focus on trading logic rather than technical details. It is especially useful for typical use cases where fine-tuning of data processing or visualization is not required.

Combined with the strategy parameter system, event model, and position protection mechanisms, the high-level API makes StockSharp a powerful and convenient tool for algorithmic trading, suitable for both beginners and experienced developers.