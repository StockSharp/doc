# ????

## ??

StockSharp ??????????????????????????????????????????????????????????????????????????????????????????

??????????

- **Brute force** -- `BruteForceOptimizer` ??????????????????????
- **Genetic algorithm** -- `GeneticOptimizer` ??????????????????????????????

????????? `BaseOptimizer`??????????????????????? `IAsyncEnumerable` ?????

## ????

### ??????????

?????????? `StrategyParam<T>` ???????????????`SetOptimize(from, to, step)` ????????????`SetCanOptimize(true)` ?????????

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

### ???????

??????????????????`decimal`?`TimeSpan`??????? `DataType` ????????????????????????????????

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

## Brute force ??

Brute force ??????????????????????????????????????????????????????

### ????????

?? `BruteForceOptimizer` ?????????????????????????????????????????????????????????

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

### ?? brute force ??

?????????????????? `ToBruteForce` ??????????? `RunAsync` ???????????????????????????

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

### ????

???????????????????????????????????????????

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

## Genetic ??

?????????????????????????????????????????????????????????????????????? brute force ????

### ????????

?? `GeneticOptimizer` ???? brute force ?????????????????????

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

### ??????

???????????????????

- `PopulationSize` - ?????????
- `Iterations` - ?????
- `MutationProbability` - ?????
- `CrossoverProbability` - ?????
- `Fitness` - ???????????????????

????????????????????????????????????????????????????

### ?????

??????????????????????? PnL????????????????????????Sharpe ????????????????????????

### ?? genetic ??

????????????????????????????????????????????????????

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

## ???????

???????????????????????????????????????????????????????????

## ????

??????????????????????????????????????????

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## ?????

???????????????????????? UI????????????????????????????

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration complete: PnL={strategy.PnL}");
};
```

## ?????

????????????????????????????????????

```csharp
// Pause. Current iterations will finish, new ones will not start.
optimizer.Pause();

// Resume.
optimizer.Resume();

// Check state.
bool isPaused = optimizer.IsPaused;
```

??????????????????????

```csharp
cts.Cancel();
```

## ?????????????

????????????????????????????????????????????

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

## ????

- [??????](historical_data.md)
- ???`Samples/07_Testing/02_Optimization`
