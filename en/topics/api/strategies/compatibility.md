# Strategy Compatibility with StockSharp Platforms

When developing trading strategies in StockSharp, it's important to consider their compatibility with various platforms: [Designer](../../designer.md), [Shell](../../shell.md), [Runner](../../runner.md), and [cloud backtesting](../../designer/backtesting/cloud_backtesting.md). By following the recommendations below, you'll create a strategy that works correctly in all environments.

## Strategy Constructor Parameters

### Avoid Parameters in the Constructor

To ensure compatibility with StockSharp platforms, especially with cloud backtesting, **you should not add parameters to the strategy constructor**:

```cs
// Correct: constructor without parameters
public class SmaStrategy : Strategy
{
    public SmaStrategy()
    {
        // Parameters initialization
    }
}

// Incorrect: constructor with parameters
public class SmaStrategy : Strategy
{
    public SmaStrategy(int longLength, int shortLength) // Don't use this approach
    {
        // ...
    }
}
```

StockSharp platforms create strategy instances using a parameterless constructor. If your strategy requires a constructor with parameters, it won't be correctly initialized.

## Using StrategyParam Instead of Regular Properties

### Benefits of StrategyParam

Instead of creating regular C# properties and then overriding the `Save` and `Load` methods, use [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) for all customizable parameters:

```cs
// Correct: using StrategyParam
private readonly StrategyParam<int> _longSmaLength;

public int LongSmaLength
{
    get => _longSmaLength.Value;
    set => _longSmaLength.Value = value;
}

public SmaStrategy()
{
    _longSmaLength = Param(nameof(LongSmaLength), 80)
                      .SetDisplay("Long SMA length", string.Empty, "Base settings");
}

// Incorrect: using regular properties
private int _longSmaLength = 80; // Don't use this approach

public int LongSmaLength
{
    get => _longSmaLength;
    set => _longSmaLength = value;
}
```

Parameters created through [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1) will automatically:
- Be displayed in the platform user interfaces
- Be saved and loaded without overriding the `Save` and `Load` methods
- Be used in optimization
- Be correctly serialized when sent to cloud backtesting

## Working with the User Interface

### Use Abstractions Instead of Direct UI Access

Instead of directly accessing user interface elements, use the abstractions provided by StockSharp:

```cs
// Correct approach: using IChart
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);
    
    // Get the chart provided by the runtime environment
    _chart = GetChart();
    
    if (_chart != null)
    {
        // Chart is available (e.g., in Designer or Shell)
        InitChart();
    }
    else
    {
        // Chart is unavailable (e.g., in Runner or cloud backtesting)
        // Strategy continues to work without visualization
    }
}

private void InitChart()
{
    // Configure chart through the abstract interface
    _chart.ClearAreas();
    var area = _chart.AddArea();
    _chartCandleElement = area.AddCandles();
    // ...
}
```

The [Strategy.GetChart()](xref:StockSharp.Algo.Strategies.Strategy.GetChart) method returns an [IChart](xref:StockSharp.Charting.IChart) interface if a chart is available in the current runtime environment. If the strategy is running in console [Runner](../../runner.md) or cloud backtesting, where there's no graphical interface, the method will return `null`.

The [IChart](xref:StockSharp.Charting.IChart) interface provides methods for working with charts:
- [AddArea](xref:StockSharp.Charting.IChart.AddArea(StockSharp.Charting.IChartArea)) - to add an area to the chart
- [RemoveArea](xref:StockSharp.Charting.IChart.RemoveArea(StockSharp.Charting.IChartArea)) - to remove an area
- [AddElement](xref:StockSharp.Charting.IChart.AddElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - to add an element to the chart
- [RemoveElement](xref:StockSharp.Charting.IChart.RemoveElement(StockSharp.Charting.IChartArea,StockSharp.Charting.IChartElement)) - to remove an element
- [Reset](xref:StockSharp.Charting.IChart.Reset(System.Collections.Generic.IEnumerable{StockSharp.Charting.IChartElement})) - to reset element values

### Check Chart Availability

Always check chart availability before using it:

```cs
private void DrawCandlesAndIndicators(ICandleMessage candle, IIndicatorValue longSma, IIndicatorValue shortSma)
{
    if (_chart == null) return; // Important check
    
    var data = _chart.CreateData();
    data.Group(candle.OpenTime)
        .Add(_chartCandleElement, candle)
        .Add(_longSmaIndicatorElement, longSma)
        .Add(_shortSmaIndicatorElement, shortSma);
    _chart.Draw(data);
}
```

## Threads and Synchronization

### Avoid Creating Additional Threads

In StockSharp, **you don't need to create additional threads** for data processing. All events (market data, transactions) come in a single thread:

```cs
// Correct: using standard event handlers
private void ProcessCandle(ICandleMessage candle)
{
    // Process candle in the main thread
    var longSmaIsFormedPrev = _longSma.IsFormed;
    var ls = _longSma.Process(candle);
    var ss = _shortSma.Process(candle);
    
    // ...
}

// Incorrect: creating additional threads
private void ProcessCandle(ICandleMessage candle)
{
    // DON'T do this
    Task.Run(() => {
        var longSmaIsFormedPrev = _longSma.IsFormed;
        // ...
    });
}
```

### Avoid Synchronization Objects

Since all events are processed in a single thread, **there's no need to use synchronization objects**:

```cs
// Correct: regular processing without synchronization
private void ProcessCandle(ICandleMessage candle)
{
    var ls = _longSma.Process(candle);
    var ss = _shortSma.Process(candle);
    // ...
}

// Incorrect: unnecessary synchronization
private readonly object _syncLock = new object(); // Not needed

private void ProcessCandle(ICandleMessage candle)
{
    lock (_syncLock) // Not needed
    {
        var ls = _longSma.Process(candle);
        // ...
    }
}
```

## External Resources

### Use StockSharp Infrastructure

Instead of directly accessing external resources (files, databases, network), use the capabilities provided by StockSharp platforms:

```cs
// Correct: using built-in mechanisms for data saving
protected override void OnStopped()
{
    // Data is automatically saved through strategy parameters
    base.OnStopped();
}

// Incorrect: direct access to external resources
protected override void OnStopped()
{
    // DON'T do this
    File.WriteAllText("results.txt", $"PnL: {PnL}");
    
    // or this
    using (var connection = new SqlConnection("..."))
    {
        // ...
    }
    
    base.OnStopped();
}
```

### Data Storage

To save strategy results, use:

- [Strategy parameters](parameters.md) for settings and configuration
- Built-in storage mechanisms in [Designer](../../designer.md) and [Shell](../../shell.md)
- [Statistics](xref:StockSharp.Algo.Statistics.StatisticManager) for collecting trading metrics

### Save and Load Methods

The [Strategy.Save](xref:StockSharp.Algo.Strategies.Strategy.Save(Ecng.Serialization.SettingsStorage)) and [Strategy.Load](xref:StockSharp.Algo.Strategies.Strategy.Load(Ecng.Serialization.SettingsStorage)) methods are specifically designed for saving additional strategy data that are not settings or parameters. This is the ideal place to save data needed to restore the strategy state:

```cs
public override void Save(SettingsStorage settings)
{
    base.Save(settings); // First save strategy parameters
    
    // Then save custom data
    settings.SetValue("CustomState", _customState);
    settings.SetValue("LastSignalTime", _lastSignalTime);
}

public override void Load(SettingsStorage settings)
{
    base.Load(settings); // First load strategy parameters
    
    // Then load custom data
    if (settings.Contains("CustomState"))
        _customState = settings.GetValue<string>("CustomState");
    
    if (settings.Contains("LastSignalTime"))
        _lastSignalTime = settings.GetValue<DateTimeOffset>("LastSignalTime");
}
```

However, the main configurable parameters should still be implemented through [StrategyParam\<T\>](xref:StockSharp.Algo.Strategies.StrategyParam`1), as described above, as this will ensure their automatic display in the user interface.

## Market Data Subscription

### Use Rules Instead of Direct Subscription

For market data processing, it's recommended to use the [Event Model](event_model.md) and rules:

```cs
protected override void OnStarted(DateTimeOffset time)
{
    base.OnStarted(time);

    _shortSma = new SimpleMovingAverage { Length = ShortSmaLength };
    _longSma = new SimpleMovingAverage { Length = LongSmaLength };

    Indicators.Add(_shortSma);
    Indicators.Add(_longSma);
    
    var subscription = new Subscription(Series, Security);

    // Correct: using rules for data processing
    Connector
        .WhenCandlesFinished(subscription)
        .Do(ProcessCandle)
        .Apply(this);

    Connector.Subscribe(subscription);
}
```

Rules have several important advantages over regular event handlers:

1. **Automatic unsubscription** - rules automatically unsubscribe from events when the strategy stops or when they're no longer needed. You don't need to manually manage subscriptions.

2. **High-level API** - rules provide a more understandable and convenient interface than standard event handlers. For example, `WhenCandlesFinished` is much clearer than subscribing to the `CandleReceived` event with subsequent checking of the candle state.

3. **Condition combining** - rules can be combined using operators like `And`, `Or`, and others, creating complex activation conditions:

```cs
// Example of combining rules
Security
    .WhenNewTrade()
    .And(Portfolio.WhenMoneyChanged())
    .Do(() => {
        // Code that executes only when there's a new trade
        // AND the portfolio balance changes
    })
    .Apply(this);
```

4. **Lifecycle management** - rules can be made one-time (`Once()`), have cancellation conditions set (`Until()`), add delayed actions, etc.

## Compatible Strategy Example

Below is an example of a strategy that follows all recommendations and will work correctly on all StockSharp platforms:

```cs
public class SmaStrategy : Strategy
{
    private readonly StrategyParam<DataType> _series;
    private readonly StrategyParam<int> _longSmaLength;
    private readonly StrategyParam<int> _shortSmaLength;

    public DataType Series
    {
        get => _series.Value;
        set => _series.Value = value;
    }

    public int LongSmaLength
    {
        get => _longSmaLength.Value;
        set => _longSmaLength.Value = value;
    }

    public int ShortSmaLength
    {
        get => _shortSmaLength.Value;
        set => _shortSmaLength.Value = value;
    }

    private SimpleMovingAverage _longSma;
    private SimpleMovingAverage _shortSma;
    private IChart _chart;
    private IChartCandleElement _chartCandleElement;
    private IChartIndicatorElement _longSmaIndicatorElement;
    private IChartIndicatorElement _shortSmaIndicatorElement;

    public SmaStrategy()
    {
        _longSmaLength = Param(nameof(LongSmaLength), 80)
                          .SetDisplay("Long SMA length", string.Empty, "Base settings")
                          .SetCanOptimize(true);
                          
        _shortSmaLength = Param(nameof(ShortSmaLength), 30)
                          .SetDisplay("Short SMA length", string.Empty, "Base settings")
                          .SetCanOptimize(true);
                          
        _series = Param(nameof(Series), DataType.TimeFrame(TimeSpan.FromMinutes(15)))
                 .SetDisplay("Series", string.Empty, "Base settings");
    }

    protected override void OnStarted(DateTimeOffset time)
    {
        base.OnStarted(time);

        _longSma = new SimpleMovingAverage { Length = LongSmaLength };
        _shortSma = new SimpleMovingAverage { Length = ShortSmaLength };

        Indicators.Add(_shortSma);
        Indicators.Add(_longSma);
        
        // Initialize chart if available
        _chart = GetChart();
        if (_chart != null)
            InitChart();
        
        var subscription = new Subscription(Series, Security);

        Connector
            .WhenCandlesFinished(subscription)
            .Do(ProcessCandle)
            .Apply(this);

        Connector.Subscribe(subscription);
    }

    private void InitChart()
    {
        _chart.ClearAreas();
        var area = _chart.AddArea();
        
        _chartCandleElement = area.AddCandles();
        
        _longSmaIndicatorElement = area.AddIndicator(_longSma);
        _longSmaIndicatorElement.Color = System.Drawing.Color.Brown;
        _longSmaIndicatorElement.DrawStyle = DrawStyles.Line;
        
        _shortSmaIndicatorElement = area.AddIndicator(_shortSma);
        _shortSmaIndicatorElement.Color = System.Drawing.Color.Blue;
        _shortSmaIndicatorElement.DrawStyle = DrawStyles.Line;
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        var ls = _longSma.Process(candle);
        var ss = _shortSma.Process(candle);
        
        // Draw on chart if available
        if (_chart != null)
        {
            var data = _chart.CreateData();
            data.Group(candle.OpenTime)
                .Add(_chartCandleElement, candle)
                .Add(_longSmaIndicatorElement, ls)
                .Add(_shortSmaIndicatorElement, ss);
            _chart.Draw(data);
        }
        
        if (!_longSma.IsFormed)
            return;
            
        var isShortLessCurrent = _shortSma.GetCurrentValue() < _longSma.GetCurrentValue();
        var isShortLessPrev = _shortSma.GetValue(1) < _longSma.GetValue(1);

        if (isShortLessCurrent == isShortLessPrev)
            return;
            
        // Trading logic
        var volume = Volume + Math.Abs(Position);

        if (isShortLessCurrent)
            SellMarket(volume);
        else
            BuyMarket(volume);
    }
}
```

## See also

- [Strategy Parameters](parameters.md)
- [Event Model](event_model.md)
- [Strategy Logging](logging.md)
