# Strategy Reports

## Overview

StockSharp provides a report generation system for strategy trading results. The system is built on two key components:

- **`IReportSource`** -- an interface describing the data source for the report (strategy parameters, orders, trades, positions, statistics).
- **`IReportGenerator`** -- a report generator interface supporting various formats (CSV, JSON, XML, Excel).

The `Strategy` class implements the `IReportSource` interface, so a strategy can be passed directly to the report generator.

## IReportSource Interface

The `IReportSource` interface provides all the data needed to generate a report:

| Property | Type | Description |
|----------|------|-------------|
| `Name` | `string` | Strategy name |
| `TotalWorkingTime` | `TimeSpan` | Total working time |
| `Commission` | `decimal?` | Total commission |
| `Position` | `decimal` | Current position |
| `PnL` | `decimal` | Total profit/loss |
| `Slippage` | `decimal?` | Total slippage |
| `Latency` | `TimeSpan?` | Total latency |
| `Parameters` | `IEnumerable<(string, object)>` | Strategy parameters |
| `StatisticParameters` | `IEnumerable<(string, object)>` | Statistical parameters |
| `Orders` | `IEnumerable<ReportOrder>` | Orders |
| `OwnTrades` | `IEnumerable<ReportTrade>` | Own trades |
| `Positions` | `IEnumerable<ReportPosition>` | Position round-trips |

Before reading data, the `Prepare()` method is called to synchronize the source's internal state.

## ReportSource Class

`ReportSource` is a standalone implementation of `IReportSource`, not tied to the `Strategy` class. It allows manually constructing the data source for a report:

```csharp
var source = new ReportSource();
source.Name = "My strategy";
source.PnL = 15000m;
source.TotalWorkingTime = TimeSpan.FromHours(8);

source.AddParameter("Timeframe", "5 minutes");
source.AddStatisticParameter("Sharpe Ratio", 1.85);

source.AddOrder(new ReportOrder(
    Id: 123,
    TransactionId: 456,
    SecurityId: securityId,
    Side: Sides.Buy,
    Time: DateTime.UtcNow,
    Price: 100m,
    State: OrderStates.Done,
    Balance: 0,
    Volume: 10,
    Type: OrderTypes.Limit
));
```

### Data Aggregation

With a large number of orders and trades, `ReportSource` automatically aggregates data to reduce the report size:

```csharp
// Automatic aggregation threshold (default is 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// Grouping interval (default is 1 hour)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// Manual aggregation
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

During aggregation, orders and trades are grouped by time interval, security, and direction. Volumes are summed, and prices are calculated as weighted averages.

## PositionLifecycleTracker

`PositionLifecycleTracker` tracks the lifecycle of positions and generates round-trips -- records of position opening and closing.

A round-trip is recorded when:
- A position is fully closed (the value becomes zero)
- A position reversal occurs (sign change)

In the `Strategy` class, the tracker is automatically integrated: completed round-trips are added to `ReportSource` via the `RoundTripClosed` event.

```csharp
var tracker = new PositionLifecycleTracker();

// Event on round-trip close
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Position closed: {roundTrip.SecurityId}, " +
        $"Open: {roundTrip.OpenTime} at {roundTrip.OpenPrice}, " +
        $"Close: {roundTrip.CloseTime} at {roundTrip.ClosePrice}, " +
        $"Max volume: {roundTrip.MaxPosition}");
};

// Process position update
tracker.ProcessPosition(position);

// Access round-trip history
IReadOnlyList<ReportPosition> history = tracker.History;
```

## Report Generators

The following generators are available:

| Generator | Format | Description |
|-----------|--------|-------------|
| `CsvReportGenerator` | CSV | Text format with delimiters |
| `JsonReportGenerator` | JSON | Structured JSON |
| `XmlReportGenerator` | XML | XML format |
| `ExcelReportGenerator` | Excel | Excel format (requires `IExcelWorkerProvider`) |

All generators inherit from `BaseReportGenerator` and support configuration of included sections:

```csharp
var generator = new CsvReportGenerator();

// Configure report sections
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## Generating a Report from a Strategy

Since `Strategy` implements `IReportSource`, a report can be generated directly:

```csharp
// The strategy itself is the data source
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

For a separate data source:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// Add positions from the tracker
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## Example: Strategy with Report Generation on Stop

```csharp
public class ReportingStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public ReportingStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

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

    protected override void OnStopped()
    {
        // Generate report when the strategy stops
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

In this example, the strategy automatically creates a CSV report when it stops. The report includes strategy parameters, statistics, orders, trades, and position round-trips.
