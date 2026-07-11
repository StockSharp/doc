# Strategieoptimierung

## Überblick

StockSharp stellt einen integrierten Mechanismus zur Optimierung von Strategieparametern auf historischen Daten bereit. Die Optimierung durchläuft automatisch verschiedene Kombinationen von Strategieparametern, etwa Indikatorlängen, Zeitrahmen, Schwellenwerte und andere Einstellungen, und hilft, das beste Ergebnis nach dem gewählten Kriterium zu finden, beispielsweise Gewinn, Drawdown, Anzahl der Trades usw.

Es stehen zwei Optimierungsmodi zur Verfügung:

- **Brute Force** -- die Klasse `BruteForceOptimizer`. Durchläuft alle möglichen Parameterkombinationen oder eine zufällige Teilmenge davon.
- **Genetischer Algorithmus** -- die Klasse `GeneticOptimizer`. Verwendet einen evolutionären Ansatz zum Finden optimaler Parameter, der bei großen Parameterräumen deutlich effizienter ist.

Beide Optimierer erben von `BaseOptimizer` und arbeiten asynchron. Sie geben ein `IAsyncEnumerable` mit Ergebnissen zurück, sobald die jeweilige Iteration abgeschlossen ist.

## Strategie vorbereiten

### Parameter mit Optimierungsbereichen definieren

Um eine Strategie zu optimieren, definieren Sie ihre Parameter über `StrategyParam<T>` und geben Wertebereiche an. Die Methode `SetOptimize(from, to, step)` legt den zu durchlaufenden Bereich fest, und `SetCanOptimize(true)` aktiviert die Optimierung für den Parameter:

```csharp
class SmaStrategy : Strategy
{
    private bool? _isShortLessThenLong;

    public SmaStrategy()
    {
        _longSma = Param(nameof(LongSma), 80)
            .SetCanOptimize(true)
            .SetOptimize(50, 100, 5);      // von 50 bis 100 mit Schrittweite 5

        _shortSma = Param(nameof(ShortSma), 30)
            .SetCanOptimize(true)
            .SetOptimize(20, 40, 1);        // von 20 bis 40 mit Schrittweite 1

        _candleTimeFrame = Param<TimeSpan?>(nameof(CandleTimeFrame))
            .SetCanOptimize(true)
            .SetOptimize(
                TimeSpan.FromMinutes(5),    // ab 5 Minuten
                TimeSpan.FromMinutes(15),   // bis 15 Minuten
                TimeSpan.FromMinutes(5));   // mit Schrittweite 5 Minuten

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

### Unterstützte Parametertypen

Die Optimierung unterstützt die folgenden Parametertypen:

| Typ | Beispiel für `SetOptimize` |
|------|-----------------------|
| `int`, `long` und andere ganzzahlige Typen | `.SetOptimize(10, 100, 5)` |
| `decimal`, `double`, `float` | `.SetOptimize(0.01m, 0.10m, 0.01m)` |
| `TimeSpan` | `.SetOptimize(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(1))` |
| `Unit` | `.SetOptimize(new Unit(1, UnitTypes.Percent), new Unit(5, UnitTypes.Percent), new Unit(0.5m, UnitTypes.Percent))` |
| `bool` | `.SetOptimize(false, true)` |

Für Parameter mit einer diskreten Wertemenge, etwa `DataType`, verwenden Sie `SetOptimizeValues`:

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

## Brute-Force-Optimierung

Die Klasse `BruteForceOptimizer` durchläuft alle möglichen Kombinationen von Parameterwerten. Dieser Modus eignet sich für kleine Parameterräume.

### Optimierer erstellen und konfigurieren

```csharp
// Instrument und Portfolio.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Speicher für historische Daten.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(folder)
};

// Optimierer erstellen.
var optimizer = new BruteForceOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry);

// Emulationsparameter konfigurieren.
var settings = optimizer.EmulationSettings;
settings.MaxIterations = 100;                          // maximale Iterationen (0 = unbegrenzt)
settings.CommissionRules = new[]                       // Kommission
{
    new CommissionTradeRule { Value = 0.01m },
};
// settings.BatchSize = 8;                             // Anzahl paralleler Threads
                                                       // Standard = CPU * 2

// Marktdaten zwischen Iterationen zwischenspeichern, um die Optimierung zu beschleunigen.
optimizer.AdapterCache = new();
```

### Brute-Force-Optimierung ausführen

```csharp
// Basisstrategie mit Optimierungsbereichen.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Zu optimierende Parameter auswählen.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

var optimizeParams = new IStrategyParam[] { longParam, shortParam, tfParam };

// Alle Parameterkombinationen erzeugen.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

// Optimierung ausführen.
var startTime = new DateTime(2020, 1, 1);
var stopTime = new DateTime(2020, 12, 31);
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(startTime, stopTime, strategies, cts.Token))
{
    // s ist die Strategie mit Ergebnissen nach dem Backtest.
    Console.WriteLine($"PnL={s.PnL}, lange SMA={s.Parameters["LongSma"].Value}, " +
                      $"kurze SMA={s.Parameters["ShortSma"].Value}");
}
```

### Zufallsstichprobe

Wenn das Durchlaufen aller Kombinationen zu viel Zeit benötigt, verwenden Sie eine Zufallsstichprobe. Die Methode `ToBruteForceRandom` erzeugt die angegebene Anzahl zufälliger Kombinationen:

```csharp
var randomCount = 50; // Anzahl zufälliger Kombinationen

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

## Genetische Optimierung

Die Klasse `GeneticOptimizer` implementiert einen genetischen Algorithmus, der bei einer großen Anzahl von Parametern deutlich effizienter ist als Brute Force. Der Algorithmus konvergiert automatisch in weniger Iterationen zu optimalen Werten.

### Optimierer erstellen und konfigurieren

```csharp
var optimizer = new GeneticOptimizer(
    new CollectionSecurityProvider(new[] { security }),
    new CollectionPortfolioProvider(new[] { portfolio }),
    storageRegistry,
    Paths.FileSystem);    // Dateisystem für die Fitness-Formel

optimizer.AdapterCache = new();

// Genetischen Algorithmus konfigurieren.
optimizer.Settings.Population = 8;            // Populationsgröße
optimizer.Settings.PopulationMax = 16;        // maximale Populationsgröße
optimizer.Settings.GenerationsMax = 20;       // maximale Anzahl Generationen
optimizer.Settings.GenerationsStagnation = 5; // nach N Generationen ohne Verbesserung stoppen
optimizer.Settings.MutationProbability = 0.1m;
optimizer.Settings.CrossoverProbability = 0.75m;
optimizer.Settings.Fitness = "PnL";           // Fitness-Formel (standardmäßig PnL)

optimizer.EmulationSettings.MaxIterations = 100;
```

### Parameter des genetischen Algorithmus

| Eigenschaft | Beschreibung | Standard |
|----------|-------------|---------|
| `Population` | Größe der Anfangspopulation | 8 |
| `PopulationMax` | Maximale Populationsgröße | 16 |
| `GenerationsMax` | Maximale Anzahl von Generationen (0 = unbegrenzt) | 20 |
| `GenerationsStagnation` | Nach N stagnierenden Generationen stoppen (0 = deaktiviert) | 5 |
| `MutationProbability` | Mutationswahrscheinlichkeit (0..1) | 0.1 |
| `CrossoverProbability` | Crossover-Wahrscheinlichkeit (0..1) | 0.75 |
| `Fitness` | Formel der Fitness-Funktion | `"PnL"` |
| `Selection` | Selektionsoperator | `TournamentSelection` |
| `Crossover` | Crossover-Operator | `OnePointCrossover` |
| `Mutation` | Mutationsoperator | `UniformMutation` |
| `Reinsertion` | Strategie zum Ersetzen von Generationen | `ElitistReinsertion` |

### Fitness-Formel

Die Eigenschaft `Fitness` definiert die Formel, mit der eine Strategie bewertet wird. Strategiestatistiken sind über Abkürzungen verfügbar:

| Abkürzung | Strategiestatistik |
|--------------|--------------------|
| `PnL` | Nettogewinn |
| `MaxDD` | Maximaler Drawdown |
| `MaxRelDD` | Maximaler relativer Drawdown |
| `WinTrades` | Gewinntrades |
| `LosTrades` | Verlusttrades |
| `Recovery` | Recovery-Faktor |
| `Ret` | Rendite |
| `TCount` | Anzahl der Trades |
| `AvgTPnL` | Durchschnittlicher Gewinn pro Trade |

Formeln können kombiniert werden, zum Beispiel: `"PnL - MaxDD"` oder `"Recovery"`.

### Genetische Optimierung ausführen

```csharp
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Parameter für den genetischen Optimierer vorbereiten.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var tfParam = (StrategyParam<TimeSpan?>)strategy.Parameters[nameof(strategy.CandleTimeFrame)];

// ToGeneticParameters konvertiert Strategieparameter in das Format des genetischen Optimierers.
// Für Parameter mit diskreter Wertemenge, etwa TimeSpan?, übergeben Sie eine explizite
// Liste über ein Tupel (param, values):
var geneticParams = strategy.ToGeneticParameters(new (IStrategyParam, IEnumerable)[]
{
    (tfParam, new[] { TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15) }),
    (longParam, null),   // null = den Bereich aus SetOptimize verwenden
    (shortParam, null),
});

// Optimierung ausführen.
var cts = new CancellationTokenSource();

await foreach (var (s, parameters) in optimizer.RunAsync(
    startTime, stopTime, strategy, geneticParams, cancellationToken: cts.Token))
{
    Console.WriteLine($"PnL={s.PnL}");
}
```

## Gemeinsame Optimierereinstellungen

Die Klasse `OptimizerSettings`, verfügbar über `optimizer.EmulationSettings`, enthält Einstellungen, die für beide Optimierungsarten gelten:

| Eigenschaft | Beschreibung | Standard |
|----------|-------------|---------|
| `BatchSize` | Anzahl parallel getesteter Strategien | `CPU * 2` |
| `MaxIterations` | Maximale Anzahl von Iterationen (0 = unbegrenzt) | 0 |
| `MaxMessageCount` | Maximale Anzahl verarbeiteter Nachrichten (-1 = unbegrenzt) | -1 |
| `CommissionRules` | Regeln zur Kommissionsberechnung | `null` |

## Daten-Caching

Die Eigenschaft `AdapterCache` speichert Marktdaten zwischen Iterationen zwischen. Das beschleunigt die Optimierung erheblich, da Daten nur einmal aus dem Speicher geladen werden:

```csharp
optimizer.AdapterCache = new MarketDataStorageCache();
```

## Optimiererereignisse

| Ereignis | Beschreibung |
|-------|-------------|
| `SingleProgressChanged` | Wird aufgerufen, wenn sich der Fortschritt einer einzelnen Iteration ändert. Parameter: `(Strategy, IStrategyParam[], int progress)`. Fortschritt `100` bedeutet, dass die Iteration abgeschlossen ist. |
| `StrategyInitialized` | Wird aufgerufen, nachdem die Strategie initialisiert wurde und bevor der Backtest startet. |
| `ConnectorInitialized` | Wird aufgerufen, nachdem der Connector erstellt wurde und bevor er eine Verbindung herstellt. Ermöglicht die Konfiguration von Parametern des `HistoryEmulationConnector`. |

```csharp
optimizer.SingleProgressChanged += (strategy, parameters, progress) =>
{
    if (progress == 100)
        Console.WriteLine($"Iteration abgeschlossen: PnL={strategy.PnL}");
};
```

## Pausieren und stoppen

Die Optimierung kann pausiert und fortgesetzt werden:

```csharp
// Pausieren. Laufende Iterationen werden abgeschlossen, neue werden nicht gestartet.
optimizer.Pause();

// Fortsetzen.
optimizer.Resume();

// Zustand prüfen.
bool isPaused = optimizer.IsPaused;
```

Um die Optimierung vollständig zu stoppen, brechen Sie das `CancellationToken` ab:

```csharp
cts.Cancel();
```

## Vollständiges Beispiel (Konsolenanwendung)

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

// Instrument und Portfolio konfigurieren.
var security = new Security
{
    Id = "AAPL@NASDAQ",
    PriceStep = 0.01m,
};

var portfolio = Portfolio.CreateSimulator();

// Datenspeicher.
var storageRegistry = new StorageRegistry
{
    DefaultDrive = new LocalMarketDataDrive(Paths.HistoryDataPath)
};

// Optimierer erstellen (Brute Force).
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

// Strategie konfigurieren.
var strategy = new SmaStrategy
{
    Volume = 1,
    Security = security,
    Portfolio = portfolio,
};

// Zu optimierende Parameter.
var longParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.LongSma)];
var shortParam = (StrategyParam<int>)strategy.Parameters[nameof(strategy.ShortSma)];
var optimizeParams = new IStrategyParam[] { longParam, shortParam };

// Kombinationen erzeugen.
var strategies = strategy.ToBruteForce(optimizeParams, out _, out var totalCount);

Console.WriteLine($"Iterationen insgesamt: {totalCount}");

// Optimierung ausführen.
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
    Console.WriteLine($"\nBestes Ergebnis: PnL={bestPnL:F2}");
    foreach (var p in bestStrategy.Parameters)
        Console.WriteLine($"  {p.Id} = {p.Value}");
}
```

## Siehe auch

- [Testing mit historischen Daten](historical_data.md)
- Beispiel: `Samples/07_Testing/02_Optimization`
