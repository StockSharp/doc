# High-Level APIs in Strategies

StockSharp provides a set of high-level APIs to simplify working with common tasks in trading strategies. These interfaces allow you to write cleaner code, focusing on trading logic rather than technical details.

## Simplified Subscription Management

High-level methods for working with subscriptions hide the complexities of managing subscription lifecycles and data processing.

### SubscribeCandles Method

Instead of manually creating a subscription and setting up event handlers, you can use the `SubscribeCandles` method:

```cs
// Creating and configuring a candle subscription in one line
var subscription = SubscribeCandles(CandleType);
```

This method returns an object of type `ISubscriptionHandler<ICandleMessage>`, which provides a convenient interface for further subscription configuration.

### Automatic Binding of Indicators to Subscription

The high-level API makes it easy to bind indicators to data subscriptions:

```cs
var longSma = new SMA { Length = Long };
var shortSma = new SMA { Length = Short };

subscription
    // Bind indicators to the candle subscription
    .Bind(longSma, shortSma, OnProcess)
    // Start processing
    .Start();
```

The `Bind` method establishes a connection between subscription data and indicators. When a new candle is received:

1. The candle is automatically sent for processing to the indicators
2. The processing results are passed to the specified handler (in the example, the `OnProcess` method)
3. All synchronization and state management code is hidden from the developer

The handler receives ready-made values as simple `decimal` types:

```cs
private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
{
    // Work directly with ready indicator values
    var isShortLessThenLong = shortValue < longValue;
    
    // Trading logic uses clean numeric values
    // without the need to extract them from IIndicatorValue
    // ...
}
```

This significantly simplifies the code and makes it more readable, as the developer doesn't need to:
- Manually handle the candle receiving event
- Pass data to indicators themselves
- Extract values from indicator results

## Simplified Chart Management

### Automatic Visualization

The high-level API provides simple methods for binding subscriptions and indicators to chart elements:

```cs
var area = CreateChartArea();

// area might be null when running without GUI
if (area != null)
{
    // Automatic binding of candles to the chart area
    DrawCandles(area, subscription);

    // Drawing indicators with color configuration
    DrawIndicator(area, shortSma, System.Drawing.Color.Coral);
    DrawIndicator(area, longSma);
    
    // Drawing own trades
    DrawOwnTrades(area);
}
```

The `DrawCandles` method automatically binds the candle subscription to the candle display element on the chart. Similarly, the `DrawIndicator` and `DrawOwnTrades` methods automatically configure the display of indicators and trades.

Benefits of this approach:
- No need to manually create `ChartDrawData` objects
- No need to manage data grouping by time
- No need to call `chart.Draw()` to update the chart

The system automatically updates the chart when new data is received, allowing the developer to focus on strategy logic rather than visualization details.

## Position Protection

### StartProtection Method

For protecting open positions, StockSharp provides the high-level `StartProtection` method:

```cs
// Starting position protection with Take Profit and Stop Loss
StartProtection(TakeValue, StopValue);
```

This method automatically sets up protection for all open positions:
- Monitors price changes
- Automatically creates orders to close positions when Take Profit or Stop Loss levels are reached
- Supports various unit types (absolute values, percentages, points)
- Can use trailing stops for adaptive position protection

Example with additional parameters:

```cs
// Starting protection with trailing stop and market orders
StartProtection(
    takeProfit: new Unit(50, UnitTypes.Point),   // Take Profit in points
    stopLoss: new Unit(2, UnitTypes.Percent),    // Stop Loss in percentage
    isStopTrailing: true,                        // Enable trailing stop
    useMarketOrders: true                        // Use market orders
);
```

## Advantages of High-Level API

The high-level API in StockSharp strategies provides the following advantages:

1. **Code Volume Reduction** - performing typical tasks requires fewer lines of code

2. **Separation of Concerns** - trading logic is separated from technical details of data processing and visualization

3. **Improved Readability** - code becomes more understandable and expressive, focused on business logic

4. **Reduced Probability of Errors** - many typical errors are eliminated through automation of routine tasks

5. **Working with Clean Data Types** - instead of working with complex objects, you can operate with simple data types (e.g., `decimal`)

## Strategy Example Using High-Level API

Below is a complete example of a strategy demonstrating the use of high-level API:

```cs
public class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _candleTypeParam = Param(nameof(CandleType), DataType.TimeFrame(TimeSpan.FromMinutes(1)));
        _long = Param(nameof(Long), 80);
        _short = Param(nameof(Short), 30);
        _takeValue = Param(nameof(TakeValue), new Unit(0, UnitTypes.Absolute));
        _stopValue = Param(nameof(StopValue), new Unit(2, UnitTypes.Percent));
    }

    private readonly StrategyParam<DataType> _candleTypeParam;
    public DataType CandleType
    {
        get => _candleTypeParam.Value;
        set => _candleTypeParam.Value = value;
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

        // Create candle subscription and bind with indicators
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

            // Place order
            if (direction == Sides.Buy)
                BuyLimit(price, volume);
            else
                SellLimit(price, volume);

            // Save current indicator positions
            _isShortLessThenLong = isShortLessThenLong;
        }
    }
}
```

## Conclusion

The high-level API in StockSharp significantly simplifies the development of trading strategies, allowing developers to focus on trading logic rather than technical details. It is especially useful for typical use cases when fine-tuning of data processing or visualization is not required.

Combined with the strategy parameter system, event model, and position protection mechanisms, the high-level API makes StockSharp a powerful and convenient tool for algorithmic trading, suitable for both beginners and experienced developers.
