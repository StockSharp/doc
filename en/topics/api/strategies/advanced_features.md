# Advanced Strategy Features

## Overview

The `Strategy` class provides a number of additional properties for fine-tuning behavior: automatic order commenting, trading schedule, risk-free rate for statistics, data source for indicators, and historical period management.

## CommentMode -- Order Comments

The `CommentMode` property controls automatic population of the `Order.Comment` field for all orders submitted by the strategy. This allows identifying which strategy created an order, which is especially useful when running multiple strategies on the same account simultaneously.

### StrategyCommentModes Enumeration

| Value | Description |
|-------|-------------|
| `Disabled` | Comment is not populated automatically. Default value. |
| `Id` | The comment is set to `Strategy.Id` (unique GUID identifier). |
| `Name` | The comment is set to `Strategy.Name` (strategy name). |

### Example

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // All orders will be tagged with the strategy name
        CommentMode = StrategyCommentModes.Name;

        // Or with the identifier for exact binding
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

With the `Name` value and a strategy name of "SMA Crossover", every order will receive the comment "SMA Crossover", allowing you to filter this strategy's orders in the trade journal.

## WorkingTime -- Working Schedule

The `WorkingTime` property sets the schedule during which the strategy is active. Outside of the specified time intervals, the strategy can automatically restrict its activity.

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Configure working time
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // Trade from 10:00 to 18:00
                        new Range<TimeSpan>(
                            TimeSpan.FromHours(10),
                            TimeSpan.FromHours(18))
                    }
                }
            }
        };
    }
}
```

The `TotalWorkingTime` property (read-only) shows the total working time of the strategy since it started. It is automatically calculated on strategy stop and restart.

## RiskFreeRate -- Risk-Free Rate

The `RiskFreeRate` property sets the annual risk-free rate used in statistical calculations -- primarily the Sharpe ratio and the Sortino ratio.

```csharp
var strategy = new MyStrategy();

// Risk-free rate of 5% per annum
strategy.RiskFreeRate = 0.05m;
```

The value is automatically passed to all statistic parameters implementing `IRiskFreeRateStatisticParameter` when the strategy's statistics manager is initialized.

## IndicatorSource -- Indicator Data Source

The `IndicatorSource` property sets the default value for the `IIndicator.Source` property for all strategy indicators that do not have an explicitly specified source. It defines which `Level1Fields` field to use as indicator input data.

```csharp
var strategy = new MyStrategy();

// All indicators will use the last trade price by default
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// Or the average price
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

If the property is `null` (the default value), indicators use their own data source.

## HistoryCalculated -- Computed Historical Period

The virtual property `HistoryCalculated` allows a strategy to programmatically determine the required historical data period for indicator warm-up. It returns `TimeSpan?` -- the duration of the historical period, or `null` if no period is specified.

```csharp
public class SmaCrossStrategy : Strategy
{
    private readonly StrategyParam<int> _longPeriod;

    public int LongPeriod
    {
        get => _longPeriod.Value;
        set => _longPeriod.Value = value;
    }

    public SmaCrossStrategy()
    {
        _longPeriod = Param(nameof(LongPeriod), 50);
    }

    // Automatic calculation of the required historical period
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` is the code-computed version of the `HistorySize` property. The difference is that `HistorySize` is set by the user as a strategy parameter, while `HistoryCalculated` is computed programmatically based on strategy parameters (for example, indicator periods).

## Example: Strategy with All Advanced Settings

```csharp
public class AdvancedStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<int> _smaPeriod;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public int SmaPeriod
    {
        get => _smaPeriod.Value;
        set => _smaPeriod.Value = value;
    }

    public AdvancedStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _smaPeriod = Param(nameof(SmaPeriod), 20);
    }

    // Automatic historical period calculation
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Order comments -- strategy name
        CommentMode = StrategyCommentModes.Name;

        // Risk-free rate for Sharpe calculation
        RiskFreeRate = 0.05m;

        // Data source for indicators
        IndicatorSource = Level1Fields.LastTradePrice;

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Trading logic...
    }
}
```

In this example, the strategy uses all the described features: it automatically comments orders, sets the risk-free rate for statistics, establishes the data source for indicators, and calculates the required historical period.
