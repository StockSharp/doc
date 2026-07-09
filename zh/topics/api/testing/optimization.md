# 策略优化

## 概述

StockSharp 提供了内置机制，可在历史数据上优化策略参数。优化过程会自动遍历策略参数的不同组合，例如指标周期、时间周期、阈值以及其他设置，并帮助按选定标准找到最佳结果，例如利润、回撤、交易次数等。

可用的优化模式有两种：

- **Brute force** -- `BruteForceOptimizer` 类。遍历所有可能的参数组合，或遍历其中的随机子集。
- **Genetic algorithm** -- `GeneticOptimizer` 类。使用进化算法寻找最优参数，对于较大的参数空间效率更高。

两个优化器都继承自 `BaseOptimizer`，并以异步方式运行，在每次迭代完成时通过 `IAsyncEnumerable` 返回结果。

## 准备策略

### 定义带优化范围的参数

要优化策略，需要通过 `StrategyParam<T>` 定义参数并指定取值范围。`SetOptimize(from, to, step)` 方法设置需要遍历的范围，`SetCanOptimize(true)` 为参数启用优化：

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

### 支持的参数类型

优化支持以下参数类型：

| 类型 | `SetOptimize` 示例 |
|------|-----------------------|
| `int`、`long` 以及其他整数类型 | `.SetOptimize(10, 100, 5)` |
| `decimal`、`double`、`float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

对于具有离散取值集合的参数，例如 `DataType`，请使用 `SetOptimizeValues`：

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

## Brute force 优化

`BruteForceOptimizer` 类会遍历参数值的所有可能组合。该模式适用于较小的参数空间。

### 创建并配置优化器

```csharp
// 工具和投资组合。
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// 历史数据存储。
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// 创建优化器。
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// 配置仿真参数。
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // maximum iterations (0 = unlimited)
settings.CommissionRules = new[]                       // commission
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // number of parallel threads
                                                       // default = CPU * 2

// 在迭代之间缓存市场数据以加快优化。
optimizer.AdapterCache = new();
```

### 运行 brute force 优化

```csharp
// 带优化范围的基础策略。
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 选择要优化的参数。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// 生成所有参数组合。
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// 运行优化。
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s 是回测后带有结果的策略。
    Console.WriteLine($"PnL={s.PnL}, LongSma={s.Parameters["LongSma"].Value}, " +
                      $"ShortSma={s.Parameters["ShortSma"].Value}");
}
```

### 随机抽样

如果遍历所有组合耗时过长，可以使用随机抽样。`ToBruteForceRandom` 方法会生成指定数量的随机组合：

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

## Genetic 优化

`GeneticOptimizer` 类实现了遗传算法。当参数数量较多时，它比 brute force 更高效。该算法会自动在较少迭代中向最优值收敛。

### 创建并配置优化器

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // file system for the fitness formula

optimizer.AdapterCache = new();

// 配置遗传算法。
optimizer.Settings.Population = 8;            // population size
optimizer.Settings.PopulationMax = 16;        // maximum population size
optimizer.Settings.GenerationsMax = 20;       // maximum generations
optimizer.Settings.GenerationsStagnation = 5; // stop after N generations without improvement
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // fitness formula (PnL by default)

optimizer.EmulationSettings.MaxIterations = 100;
```

### 遗传算法参数

| 属性 | 描述 | 默认值 |
|----------|-------------|---------|
| `Population` | 初始种群大小 | 8 |
| `PopulationMax` | 最大种群大小 | 16 |
| `GenerationsMax` | 最大代数（0 = 不限制） | 20 |
| `GenerationsStagnation` | 连续 N 代无改进后停止（0 = 禁用） | 5 |
| `MutationProbability` | 变异概率（0..1） | 0.1 |
| `CrossoverProbability` | 交叉概率（0..1） | 0.75 |
| `Fitness` | 适应度函数公式 | `"PnL"` |
| `Selection` | 选择算子 | `TournamentSelection` |
| `Crossover` | 交叉算子 | `OnePointCrossover` |
| `Mutation` | 变异算子 | `UniformMutation` |
| `Reinsertion` | 代际替换策略 | `ElitistReinsertion` |

### 适应度公式

`Fitness` 属性定义用于评估策略的公式。策略统计指标可通过缩写访问：

| 缩写 | 策略统计指标 |
|--------------|--------------------|
| `PnL` | 净利润 |
| `MaxDD` | 最大回撤 |
| `MaxRelDD` | 最大相对回撤 |
| `WinTrades` | 盈利交易 |
| `LosTrades` | 亏损交易 |
| `Recovery` | 恢复因子 |
| `Ret` | 收益率 |
| `TCount` | 交易次数 |
| `AvgTPnL` | 每笔交易平均利润 |

公式可以组合使用，例如 `"PnL - MaxDD"` 或 `"Recovery"`。

### 运行 genetic 优化

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 为遗传优化器准备参数。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters 将策略参数转换为遗传优化器格式。
// For parameters with a discrete set of values, such as TimeSpan?, pass an explicit
// list through a (param, values) tuple:
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = use the range from SetOptimize
    (shortParam, null),
});

// 运行优化。
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## 通用优化器设置

`OptimizerSettings` 类可通过 `optimizer.EmulationSettings` 访问，其中包含两种优化类型通用的设置：

| 属性 | 描述 | 默认值 |
|----------|-------------|---------|
| `BatchSize` | 并行测试的策略数量 | `CPU * 2` |
| `MaxIterations` | 最大迭代次数（0 = 不限制） | 0 |
| `MaxMessageCount` | 最大处理消息数（-1 = 不限制） | -1 |
| `CommissionRules` | 佣金计算规则 | `null` |

## 数据缓存

`AdapterCache` 属性会在迭代之间缓存市场数据。由于数据只从存储中加载一次，这可以显著加快优化速度：

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## 优化器事件

| 事件 | 描述 |
|-------|-------------|
| `SingleProgressChanged` | 单次迭代进度变化时调用。参数：`(Strategy, IStrategyParam[], int progress)`。进度为 `100` 表示该迭代已完成。 |
| `StrategyInitialized` | 策略初始化后、回测开始前调用。 |
| `ConnectorInitialized` | 连接器创建后、连接前调用。允许配置 `HistoryEmulationConnector` 参数。 |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration complete: PnL={strategy.PnL}");
};
```

## 暂停和停止

优化可以暂停和恢复：

```csharp
// 暂停。当前迭代会完成，新迭代不会启动。
optimizer.Pause();

// Resume.
optimizer.Resume();

// 检查状态。
bool isPaused = optimizer.IsPaused;
```

要完全停止优化，请取消 `CancellationToken`：

```csharp
cts.Cancel();
```

## 完整示例（控制台应用程序）

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

// 配置工具和投资组合。
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// 数据存储。
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// 创建优化器（暴力搜索）。
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

// 配置策略。
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 要优化的参数。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// 生成组合。
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Total iterations: {totalCount}");

// 运行优化。
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

## 另请参阅

- [历史数据测试](historical_data.md)
- 示例：`Samples/07_Testing/02_Optimization`
