# ストラテジー最適化

## 概要

StockSharp は、履歴データ上でストラテジーパラメーターを最適化するための組み込みメカニズムを提供します。最適化は、インジケーター期間、時間枠、しきい値、その他の設定など、ストラテジーパラメーターのさまざまな組み合わせを自動的に反復し、利益、ドローダウン、取引数など、選択した基準に対して最良の結果を見つけるのに役立ちます。

2 つの最適化モードを利用できます。

- **総当たり** -- `BruteForceOptimizer` クラス。すべての可能なパラメーター組み合わせ、またはそのランダムなサブセットを反復します。
- **遺伝的アルゴリズム** -- `GeneticOptimizer` クラス。大きなパラメーター空間でより効率的に最適なパラメーターを見つけるため、進化的アプローチを使用します。

どちらの最適化器も `BaseOptimizer` から継承し、非同期で動作し、各反復が完了するたびに結果を含む `IAsyncEnumerable` を返します。

## ストラテジーの準備

### 最適化範囲を持つパラメーターの定義

ストラテジーを最適化するには、`StrategyParam<T>` を通じてパラメーターを定義し、値の範囲を指定します。`SetOptimize(from, to, step)` メソッドは反復する範囲を設定し、`SetCanOptimize(true)` はそのパラメーターの最適化を有効にします。

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

### サポートされるパラメーター型

最適化は以下のパラメーター型をサポートします。

| 型 | `SetOptimize` の例 |
|------|-----------------------|
| `int`、`long`、その他の整数型 | `.SetOptimize(10, 100, 5)` |
| `decimal`、`double`、`float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

`DataType` など、離散的な値セットを持つパラメーターには `SetOptimizeValues` を使用します。

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

## 総当たり最適化

`BruteForceOptimizer` クラスは、パラメーター値のすべての可能な組み合わせを反復します。このモードは小さなパラメーター空間に適しています。

### 最適化器の作成と設定

```csharp
// 銘柄とポートフォリオ。
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// 履歴データストレージ。
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// オプティマイザーを作成。
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// エミュレーションパラメータを設定。
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // maximum iterations (0 = unlimited)
settings.CommissionRules = new[]                       // commission
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // number of parallel threads
                                                       // default = CPU * 2

// 最適化を高速化するため、反復間で市場データをキャッシュ。
optimizer.AdapterCache = new();
```

### 総当たり最適化の実行

```csharp
// 最適化範囲を持つベース戦略。
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 最適化するパラメータを選択。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// すべてのパラメータ組み合わせを生成。
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// 最適化を実行。
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s はバックテスト後の結果を持つ戦略です。
    Console.WriteLine($"PnL={s.PnL}, LongSma={s.Parameters["LongSma"].Value}, " +
                      $"ShortSma={s.Parameters["ShortSma"].Value}");
}
```

### ランダムサンプリング

すべての組み合わせを反復するのに時間がかかりすぎる場合は、ランダムサンプリングを使用します。`ToBruteForceRandom` メソッドは、指定された数のランダムな組み合わせを生成します。

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

## 遺伝的最適化

`GeneticOptimizer` クラスは、パラメーター数が多い場合に総当たりよりはるかに効率的な遺伝的アルゴリズムを実装します。このアルゴリズムは、より少ない反復回数で最適値へ自動的に収束します。

### 最適化器の作成と設定

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // file system for the fitness formula

optimizer.AdapterCache = new();

// 遺伝的アルゴリズムを設定。
optimizer.Settings.Population = 8;            // population size
optimizer.Settings.PopulationMax = 16;        // maximum population size
optimizer.Settings.GenerationsMax = 20;       // maximum generations
optimizer.Settings.GenerationsStagnation = 5; // stop after N generations without improvement
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // fitness formula (PnL by default)

optimizer.EmulationSettings.MaxIterations = 100;
```

### 遺伝的アルゴリズムのパラメーター

| プロパティ | 説明 | 既定値 |
|----------|-------------|---------|
| `Population` | 初期集団サイズ | 8 |
| `PopulationMax` | 最大集団サイズ | 16 |
| `GenerationsMax` | 最大世代数 (0 = 無制限) | 20 |
| `GenerationsStagnation` | N 世代停滞後に停止 (0 = 無効) | 5 |
| `MutationProbability` | 突然変異確率 (0..1) | 0.1 |
| `CrossoverProbability` | 交叉確率 (0..1) | 0.75 |
| `Fitness` | 適応度関数の式 | `"PnL"` |
| `Selection` | 選択演算子 | `TournamentSelection` |
| `Crossover` | 交叉演算子 | `OnePointCrossover` |
| `Mutation` | 突然変異演算子 | `UniformMutation` |
| `Reinsertion` | 世代置換戦略 | `ElitistReinsertion` |

### 適応度式

`Fitness` プロパティは、ストラテジーを評価するために使用する式を定義します。ストラテジー統計は略語で利用できます。

| 略語 | ストラテジー統計 |
|--------------|--------------------|
| `PnL` | 純利益 |
| `MaxDD` | 最大ドローダウン |
| `MaxRelDD` | 最大相対ドローダウン |
| `WinTrades` | 勝ち取引 |
| `LosTrades` | 負け取引 |
| `Recovery` | 回復係数 |
| `Ret` | リターン |
| `TCount` | 取引数 |
| `AvgTPnL` | 1 取引あたりの平均利益 |

式は、たとえば `"PnL - MaxDD"` や `"Recovery"` のように組み合わせることができます。

### 遺伝的最適化の実行

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 遺伝的オプティマイザー用のパラメータを準備。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters は戦略パラメータを遺伝的オプティマイザー形式に変換します。
// For parameters with a discrete set of values, such as TimeSpan?, pass an explicit
// list through a (param, values) tuple:
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = use the range from SetOptimize
    (shortParam, null),
});

// 最適化を実行。
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## 共通の最適化器設定

`optimizer.EmulationSettings` から利用できる `OptimizerSettings` クラスには、両方の最適化タイプに共通する設定が含まれます。

| プロパティ | 説明 | 既定値 |
|----------|-------------|---------|
| `BatchSize` | 並列にテストされるストラテジー数 | `CPU * 2` |
| `MaxIterations` | 最大反復回数 (0 = 無制限) | 0 |
| `MaxMessageCount` | 処理されるメッセージの最大数 (-1 = 無制限) | -1 |
| `CommissionRules` | 手数料計算ルール | `null` |

## データキャッシュ

`AdapterCache` プロパティは、反復間で市場データをキャッシュします。データがストレージから一度だけ読み込まれるため、最適化が大幅に高速化されます。

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## 最適化器イベント

| イベント | 説明 |
|-------|-------------|
| `SingleProgressChanged` | 単一反復の進捗が変化したときに呼び出されます。パラメーター: `(Strategy, IStrategyParam[], int progress)`。進捗 `100` は反復完了を意味します。 |
| `StrategyInitialized` | ストラテジーが初期化された後、バックテスト開始前に呼び出されます。 |
| `ConnectorInitialized` | コネクターが作成された後、接続前に呼び出されます。`HistoryEmulationConnector` パラメーターを設定できます。 |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration complete: PnL={strategy.PnL}");
};
```

## 一時停止と停止

最適化は一時停止して再開できます。

```csharp
// 一時停止。現在の反復は完了し、新しい反復は開始されません。
optimizer.Pause();

// Resume.
optimizer.Resume();

// 状態を確認。
bool isPaused = optimizer.IsPaused;
```

最適化を完全に停止するには、`CancellationToken` をキャンセルします。

```csharp
cts.Cancel();
```

## 完全な例 (コンソールアプリケーション)

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

// 銘柄とポートフォリオを設定。
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// データストレージ。
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

// 戦略を設定。
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// 最適化するパラメータ。
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// 組み合わせを生成。
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Total iterations: {totalCount}");

// 最適化を実行。
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

## 関連項目

- [履歴データテスト](historical_data.md)
- 例: `Samples/07_Testing/02_Optimization`
