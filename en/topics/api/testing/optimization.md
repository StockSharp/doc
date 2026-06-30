# Strategy optimization

## Overview

StockSharp provides a built-in mechanism for optimizing strategy parameters on historical data. Optimization automatically iterates through different combinations of strategy parameters, such as indicator lengths, time frames, threshold values, and other settings, and helps find the best result for the selected criterion, such as profit, drawdown, number of trades, and so on.

Two optimization modes are available:

- **Brute force** -- the `BruteForceOptimizer` class. Iterates through all possible parameter combinations, or through a random subset of them.
- **Genetic algorithm** -- the `GeneticOptimizer` class. Uses an evolutionary approach to find optimal parameters, which is much more efficient for large parameter spaces.

Both optimizers inherit from `BaseOptimizer` and work asynchronously, returning an `IAsyncEnumerable` with results as each iteration completes.

## Preparing a strategy

### Defining parameters with optimization ranges

To optimize a strategy, define its parameters through `StrategyParam<T>` and specify value ranges. The `SetOptimize(from, to, step)` method sets the range to iterate through, and `SetCanOptimize(true)` enables optimization for the parameter:

```csharp
class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _longSma = Param(nameof(LongSma), 80)
            .SetCanOptimize(true)
            .SetOptimize(50, 100, 5);      // from 50 to 100 with a step of 5

        _shortSma = Param(nameof(ShortSma), 30)
            .SetCanOptimize(true)
            .SetOptimize(20, 40, 1);        // from 20 to 40 with a step of 1

        _candleTimeFrame = Param<TimeSpan?>(nameof(CandleTimeFrame))
            .SetCanOptimize(true)
            .SetOptimize(
                TimeSpan.FromMinutes(5),    // from 5 minutes
                TimeSpan.FromMinutes(15),   // to 15 minutes
                TimeSpan.FromMinutes(5));   // with a step of 5 minutes

        _candleType = Param(nameof(CandleType),
            TimeSpan.FromMinutes(1).TimeFrame()).SetRequired();
    }

    private readonly StrategyParam<int> _longSma;
    public int LongSma
    {
        get => _longSma.Value;
        set => _longSma.Value = value;
    }

    private readonly StrategyParam<int> _shortSma;
    public int ShortSma
    {
        get => _shortSma.Value;
        set => _shortSma.Value = value;
    }

    private readonly StrategyParam<TimeSpan?> _candleTimeFrame;
    public TimeSpan? CandleTimeFrame
    {
        get => _candleTimeFrame.Value;
        set => _candleTimeFrame.Value = value;
    }

    private readonly StrategyParam<DataType> _candleType;
    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var dt = CandleTimeFrame is null
            ? CandleType
            : DataType.Create(CandleType.MessageType, CandleTimeFrame);

        var subscription = new Subscription(dt, Security)
        {
            MarketData =
            {
                IsFinishedOnly = true,
            }
        };

        var longSma = new SMA { Length = LongSma };
        var shortSma = new SMA { Length = ShortSma };

        SubscribeCandles(subscription)
            .Bind(longSma, shortSma, OnProcess)
            .Start();
    }

    private void OnProcess(ICandleMessage candle, decimal longValue, decimal shortValue)
    {
        if (candle.State != CandleStates.Finished)
            return;

        var isShortLessThenLong = shortValue < longValue;

        if (_isShortLessThenLong == null)
        {
            _isShortLessThenLong = isShortLessThenLong;
        }
        else if (_isShortLessThenLong != isShortLessThenLong)
        {
            var direction = isShortLessThenLong ? Sides.Sell : Sides.Buy;
            var volume = Position == 0 ? Volume : Position.Abs().Min(Volume) * 2;
            var price = candle.ClosePrice;

            if (direction == Sides.Buy)
                BuyLimit(price, volume);
            else
                SellLimit(price, volume);

            _isShortLessThenLong = isShortLessThenLong;
        }
    }

    protected override void OnReseted()
    {
        base.OnReseted();
        _isShortLessThenLong = null;
    }
}
```

### Supported parameter types

Optimization supports the following parameter types:

| Type | Example `SetOptimize` |
|------|-----------------------|
| `int`, `long`, and other integer types | `.SetOptimize(10, 100, 5)` |
| `decimal`, `double`, `float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

For parameters with a discrete set of values, such as `DataType`, use `SetOptimizeValues`:

```csharp
_candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame())
    .SetCanOptimize(true)
    .SetOptimizeValues(new[]
    {
        TimeSpan.FromMinutes(5).TimeFrame(),
        TimeSpan.FromMinutes(15).TimeFrame(),
        TimeSpan.FromMinutes(30).TimeFrame(),
    });
```

## Brute force optimization

The `BruteForceOptimizer` class iterates through all possible combinations of parameter values. This mode is suitable for small parameter spaces.

### Creating and configuring the optimizer

```csharp
// Instrument and portfolio.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Historical data storage.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// Create the optimizer.
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// Configure emulation parameters.
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // maximum iterations (0 = unlimited)
settings.CommissionRules = new[]                       // commission
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // number of parallel threads
                                                       // default = CPU * 2

// Cache market data between iterations to speed up optimization.
optimizer.AdapterCache = new();
```

### Running brute force optimization

```csharp
// Base strategy with optimization ranges.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Select parameters to optimize.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// Generate all parameter combinations.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// Run optimization.
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s is the strategy with results after backtesting.
    Console.WriteLine($"PnL={s.PnL}, LongSma={s.Parameters["LongSma"].Value}, " +
                      $"ShortSma={s.Parameters["ShortSma"].Value}");
}
```

### Random sampling

If iterating through all combinations takes too much time, use random sampling. The `ToBruteForceRandom` method generates the specified number of random combinations:

```csharp
var randomCount = 50; // number of random combinations

var strategies = strategy.ToBruteForceRandom(
    optimizeParams,
    randomCount,
    out _,
    out var totalCount);

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Genetic optimization

The `GeneticOptimizer` class implements a genetic algorithm that is much more efficient than brute force when the number of parameters is large. The algorithm automatically converges toward optimal values in fewer iterations.

### Creating and configuring the optimizer

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // file system for the fitness formula

optimizer.AdapterCache = new();

// Configure the genetic algorithm.
optimizer.Settings.Population = 8;            // population size
optimizer.Settings.PopulationMax = 16;        // maximum population size
optimizer.Settings.GenerationsMax = 20;       // maximum generations
optimizer.Settings.GenerationsStagnation = 5; // stop after N generations without improvement
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // fitness formula (PnL by default)

optimizer.EmulationSettings.MaxIterations = 100;
```

### Genetic algorithm parameters

| Property | Description | Default |
|----------|-------------|---------|
| `Population` | Initial population size | 8 |
| `PopulationMax` | Maximum population size | 16 |
| `GenerationsMax` | Maximum number of generations (0 = unlimited) | 20 |
| `GenerationsStagnation` | Stop after N stagnant generations (0 = disabled) | 5 |
| `MutationProbability` | Mutation probability (0..1) | 0.1 |
| `CrossoverProbability` | Crossover probability (0..1) | 0.75 |
| `Fitness` | Fitness function formula | `"PnL"` |
| `Selection` | Selection operator | `TournamentSelection` |
| `Crossover` | Crossover operator | `OnePointCrossover` |
| `Mutation` | Mutation operator | `UniformMutation` |
| `Reinsertion` | Generation replacement strategy | `ElitistReinsertion` |

### Fitness formula

The `Fitness` property defines the formula used to evaluate a strategy. Strategy statistics are available through abbreviations:

| Abbreviation | Strategy statistic |
|--------------|--------------------|
| `PnL` | Net profit |
| `MaxDD` | Maximum drawdown |
| `MaxRelDD` | Maximum relative drawdown |
| `WinTrades` | Winning trades |
| `LosTrades` | Losing trades |
| `Recovery` | Recovery factor |
| `Ret` | Return |
| `TCount` | Number of trades |
| `AvgTPnL` | Average profit per trade |

Formulas can be combined, for example: `"PnL - MaxDD"` or `"Recovery"`.

### Running genetic optimization

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Prepare parameters for the genetic optimizer.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters converts strategy parameters to the genetic optimizer format.
// For parameters with a discrete set of values, such as TimeSpan?, pass an explicit
// list through a (param, values) tuple:
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = use the range from SetOptimize
    (shortParam, null),
});

// Run optimization.
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Common optimizer settings

The `OptimizerSettings` class, available through `optimizer.EmulationSettings`, contains settings common to both optimization types:

| Property | Description | Default |
|----------|-------------|---------|
| `BatchSize` | Number of strategies tested in parallel | `CPU * 2` |
| `MaxIterations` | Maximum number of iterations (0 = unlimited) | 0 |
| `MaxMessageCount` | Maximum number of processed messages (-1 = unlimited) | -1 |
| `CommissionRules` | Commission calculation rules | `null` |

## Data caching

The `AdapterCache` property caches market data between iterations. This significantly speeds up optimization because data is loaded from storage only once:

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## Optimizer events

| Event | Description |
|-------|-------------|
| `SingleProgressChanged` | Called when progress changes for a single iteration. Parameters: `(Strategy, IStrategyParam[], int progress)`. Progress `100` means the iteration is complete. |
| `StrategyInitialized` | Called after the strategy is initialized and before the backtest starts. |
| `ConnectorInitialized` | Called after the connector is created and before it connects. Allows configuring `HistoryEmulationConnector` parameters. |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration complete: PnL={strategy.PnL}");
};
```

## Pause and stop

Optimization can be paused and resumed:

```csharp
// Pause. Current iterations will finish, new ones will not start.
optimizer.Pause();

// Resume.
optimizer.Resume();

// Check state.
bool isPaused = optimizer.IsPaused;
```

To stop optimization completely, cancel the `CancellationToken`:

```csharp
cts.Cancel();
```

## Complete example (console application)

```csharp
using System;
using System.Linq;
using System.Threading;

using StockSharp.Algo;
using StockSharp.Algo.Storages;
using StockSharp.Algo.Strategies;
using StockSharp.Algo.Strategies.Optimization;
using StockSharp.Algo.Commissions;
using StockSharp.BusinessEntities;
using StockSharp.Configuration;
using StockSharp.Messages;

// Configure the instrument and portfolio.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Data storage.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// Create the optimizer (brute force).
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

optimizer.EmulationSettings.MaxIterations = 100;
optimizer.EmulationSettings.CommissionRules = new[]
{
    new CommissionTradeRule { Value = 0.01m },
};
optimizer.AdapterCache = new();

// Configure the strategy.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Parameters to optimize.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// Generate combinations.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Total iterations: {totalCount}");

// Run optimization.
var startTime = Paths.HistoryBeginDate;
var stopTime = Paths.HistoryEndDate;
var cts = new CancellationTokenSource();

var bestPnL = decimal.MinValue;
Strategy bestStrategy = null;

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    var pnl = s.PnL;
    var paramStr = string.Join(", ", parameters.Select(p => $"{p.Id}={p.Value}"));
    Console.WriteLine($"[{paramStr}] PnL={pnl:F2}");

    if (pnl > bestPnL)
    {
        bestPnL = pnl;
        bestStrategy = s;
    }
}

if (bestStrategy != null)
{
    Console.WriteLine($"\nBest result: PnL={bestPnL:F2}");
    foreach (var p in bestStrategy.Parameters)
        Console.WriteLine($"  {p.Id} = {p.Value}");
}
```

## See also

- [Historical data testing](historical_data.md)
- Example: `Samples/07_Testing/02_Optimization`
