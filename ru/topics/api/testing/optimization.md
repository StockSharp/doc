# Оптимизация стратегий

## Обзор

StockSharp предоставляет встроенный механизм оптимизации параметров стратегий на исторических данных. Оптимизация позволяет автоматически перебирать различные комбинации параметров стратегии (длины индикаторов, таймфреймы, пороговые значения и т.д.) и находить наилучшие с точки зрения выбранного критерия (прибыль, просадка, количество сделок и т.д.).

Доступны два режима оптимизации:

- **Полный перебор (Brute Force)** -- класс `BruteForceOptimizer`. Перебирает все возможные комбинации параметров или случайную выборку из них.
- **Генетический алгоритм** -- класс `GeneticOptimizer`. Использует эволюционный подход для поиска оптимальных параметров, что значительно эффективнее при большом пространстве параметров.

Оба оптимизатора наследуют от `BaseOptimizer` и работают асинхронно, возвращая `IAsyncEnumerable` с результатами по мере завершения каждой итерации.

## Подготовка стратегии

### Определение параметров с диапазонами оптимизации

Для оптимизации параметры стратегии должны быть объявлены через `StrategyParam<T>` с указанием диапазона значений. Метод `SetOptimize(from, to, step)` задает диапазон перебора, а `SetCanOptimize(true)` разрешает оптимизацию параметра:

```csharp
class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _longSma = Param(nameof(LongSma), 80)
            .SetCanOptimize(true)
            .SetOptimize(50, 100, 5);      // от 50 до 100 с шагом 5

        _shortSma = Param(nameof(ShortSma), 30)
            .SetCanOptimize(true)
            .SetOptimize(20, 40, 1);        // от 20 до 40 с шагом 1

        _candleTimeFrame = Param<TimeSpan?>(nameof(CandleTimeFrame))
            .SetCanOptimize(true)
            .SetOptimize(
                TimeSpan.FromMinutes(5),    // от 5 минут
                TimeSpan.FromMinutes(15),   // до 15 минут
                TimeSpan.FromMinutes(5));   // с шагом 5 минут

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

### Поддерживаемые типы параметров

Оптимизация поддерживает следующие типы параметров:

| Тип | Пример `SetOptimize` |
|-----|---------------------|
| `int`, `long` и другие целочисленные | `.SetOptimize(10, 100, 5)` |
| `decimal`, `double`, `float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

Для параметров с дискретным набором значений (например, `DataType`) используйте `SetOptimizeValues`:

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

## Полный перебор (Brute Force)

Класс `BruteForceOptimizer` перебирает все возможные комбинации значений параметров. Подходит для небольших пространств параметров.

### Создание и настройка

```csharp
// Инструмент и портфель
var security = new Security
{
    Id = "SBER@TQBR",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Хранилище исторических данных
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// Создание оптимизатора
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// Настройка параметров эмуляции
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // макс. кол-во итераций (0 = без ограничения)
settings.CommissionRules = new[]                       // комиссия
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // кол-во параллельных потоков
                                                       // по умолчанию = CPU * 2

// Кэширование рыночных данных между итерациями (ускоряет работу)
optimizer.AdapterCache = new();
```

### Запуск полного перебора

```csharp
// Базовая стратегия с диапазонами оптимизации
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Выбрать параметры для оптимизации
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// Сгенерировать все комбинации параметров
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// Запустить оптимизацию
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s -- стратегия с результатами после бэктеста
    Console.WriteLine($"PnL={s.PnL}, LongSma={s.Parameters["LongSma"].Value}, " +
                      $"ShortSma={s.Parameters["ShortSma"].Value}");
}
```

### Случайная выборка

Если полный перебор всех комбинаций занимает слишком много времени, можно использовать случайную выборку. Метод `ToBruteForceRandom` генерирует заданное количество случайных комбинаций:

```csharp
var randomCount = 50; // количество случайных комбинаций

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

## Генетическая оптимизация

Класс `GeneticOptimizer` реализует генетический алгоритм, который значительно эффективнее полного перебора при большом количестве параметров. Алгоритм автоматически сходится к оптимальным значениям за меньшее число итераций.

### Создание и настройка

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // файловая система для формулы фитнеса

optimizer.AdapterCache = new();

// Настройка генетического алгоритма
optimizer.Settings.Population = 8;            // размер популяции
optimizer.Settings.PopulationMax = 16;        // максимальный размер популяции
optimizer.Settings.GenerationsMax = 20;       // макс. кол-во поколений
optimizer.Settings.GenerationsStagnation = 5; // остановка при стагнации (кол-во поколений без улучшения)
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // формула фитнеса (по умолчанию PnL)

optimizer.EmulationSettings.MaxIterations = 100;
```

### Параметры генетического алгоритма

| Свойство | Описание | По умолчанию |
|----------|----------|-------------|
| `Population` | Начальный размер популяции | 8 |
| `PopulationMax` | Максимальный размер популяции | 16 |
| `GenerationsMax` | Максимальное кол-во поколений (0 = без ограничения) | 20 |
| `GenerationsStagnation` | Остановка при стагнации N поколений (0 = отключено) | 5 |
| `MutationProbability` | Вероятность мутации (0..1) | 0.1 |
| `CrossoverProbability` | Вероятность скрещивания (0..1) | 0.75 |
| `Fitness` | Формула фитнес-функции | `"PnL"` |
| `Selection` | Оператор отбора | `TournamentSelection` |
| `Crossover` | Оператор скрещивания | `OnePointCrossover` |
| `Mutation` | Оператор мутации | `UniformMutation` |
| `Reinsertion` | Стратегия замены поколений | `ElitistReinsertion` |

### Формула фитнес-функции

Свойство `Fitness` задает формулу для оценки стратегии. Поддерживаются статистические параметры стратегии в виде сокращений:

| Сокращение | Статистический параметр |
|-----------|------------------------|
| `PnL` | Чистая прибыль |
| `MaxDD` | Максимальная просадка |
| `MaxRelDD` | Максимальная относительная просадка |
| `WinTrades` | Выигрышные сделки |
| `LosTrades` | Убыточные сделки |
| `Recovery` | Коэффициент восстановления |
| `Ret` | Доходность |
| `TCount` | Количество сделок |
| `AvgTPnL` | Средняя прибыль на сделку |

Можно комбинировать формулы, например: `"PnL - MaxDD"` или `"Recovery"`.

### Запуск генетической оптимизации

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Подготовить параметры для генетического оптимизатора
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// Метод ToGeneticParameters конвертирует параметры стратегии в формат генетического оптимизатора.
// Для параметров с дискретным набором значений (например, TimeSpan?) можно передать
// явный список через кортеж (param, values):
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = использовать диапазон из SetOptimize
    (shortParam, null),
});

// Запуск
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Общие настройки оптимизатора

Класс `OptimizerSettings` (доступен через `optimizer.EmulationSettings`) содержит настройки, общие для обоих типов оптимизации:

| Свойство | Описание | По умолчанию |
|----------|----------|-------------|
| `BatchSize` | Количество параллельно тестируемых стратегий | `CPU * 2` |
| `MaxIterations` | Максимальное количество итераций (0 = без ограничения) | 0 |
| `MaxMessageCount` | Максимальное количество обработанных сообщений (-1 = без ограничения) | -1 |
| `CommissionRules` | Правила расчета комиссии | `null` |

## Кэширование данных

Свойство `AdapterCache` позволяет кэшировать рыночные данные между итерациями, что значительно ускоряет оптимизацию, так как данные загружаются из хранилища только один раз:

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## События оптимизатора

| Событие | Описание |
|---------|----------|
| `SingleProgressChanged` | Вызывается при изменении прогресса отдельной итерации. Параметры: `(Strategy, IStrategyParam[], int progress)`. Прогресс 100 означает завершение итерации. |
| `StrategyInitialized` | Вызывается после инициализации стратегии перед запуском бэктеста. |
| `ConnectorInitialized` | Вызывается после создания коннектора перед подключением. Позволяет настроить параметры `HistoryEmulationConnector`. |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Итерация завершена: PnL={strategy.PnL}");
};
```

## Пауза и остановка

Оптимизацию можно приостановить и возобновить:

```csharp
// Приостановить (текущие итерации завершатся, новые не начнутся)
optimizer.Pause();

// Возобновить
optimizer.Resume();

// Проверить состояние
bool isPaused = optimizer.IsPaused;
```

Для полной остановки отмените `CancellationToken`:

```csharp
cts.Cancel();
```

## Полный пример (консольное приложение)

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

// Настройка инструмента и портфеля
var security = new Security
{
    Id = "SBER@TQBR",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Хранилище данных
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// Создание оптимизатора (полный перебор)
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

// Настройка стратегии
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Параметры для оптимизации
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// Генерация комбинаций
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Всего итераций: {totalCount}");

// Запуск оптимизации
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
    Console.WriteLine($"\nЛучший результат: PnL={bestPnL:F2}");
    foreach (var p in bestStrategy.Parameters)
        Console.WriteLine($"  {p.Id} = {p.Value}");
}
```

## См. также

- [Тестирование на исторических данных](historical_data.md)
- Пример: `Samples/07_Testing/02_Optimization`
