# Strategieberichte

## Überblick

StockSharp stellt ein System zur Berichtserstellung für Handelsergebnisse von Strategien bereit. Das System basiert auf zwei Schlüsselkomponenten:

- **`IReportSource`** -- eine Schnittstelle, die die Datenquelle für den Bericht beschreibt (Strategieparameter, Orders, Trades, Positionen, Statistiken).
- **`IReportGenerator`** -- eine Schnittstelle für Berichtsgeneratoren, die verschiedene Formate unterstützt (CSV, JSON, XML, Excel).

Die Klasse `Strategy` implementiert die Schnittstelle `IReportSource`, sodass eine Strategie direkt an den Berichtsgenerator übergeben werden kann.

## IReportSource-Schnittstelle

Die Schnittstelle `IReportSource` stellt alle Daten bereit, die zum Erzeugen eines Berichts benötigt werden:

| Eigenschaft | Typ | Beschreibung |
|-------------|-----|--------------|
| `Name` | `string` | Strategiename |
| `TotalWorkingTime` | `TimeSpan` | Gesamte Arbeitszeit |
| `Commission` | `decimal?` | Gesamte Kommission |
| `Position` | `decimal` | Aktuelle Position |
| `PnL` | `decimal` | Gesamter Gewinn/Verlust |
| `Slippage` | `decimal?` | Gesamte Slippage |
| `Latency` | `TimeSpan?` | Gesamte Latenz |
| `Parameters` | `IEnumerable<(string, object)>` | Strategieparameter |
| `StatisticParameters` | `IEnumerable<(string, object)>` | Statistikparameter |
| `Orders` | `IEnumerable<ReportOrder>` | Orders |
| `OwnTrades` | `IEnumerable<ReportTrade>` | Eigene Trades |
| `Positions` | `IEnumerable<ReportPosition>` | Positions-Round-Trips |

Vor dem Lesen der Daten wird die Methode `Prepare()` aufgerufen, um den internen Zustand der Quelle zu synchronisieren.

## ReportSource-Klasse

`ReportSource` ist eine eigenständige Implementierung von `IReportSource`, die nicht an die Klasse `Strategy` gebunden ist. Sie erlaubt den manuellen Aufbau der Datenquelle für einen Bericht:

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

### Datenaggregation

Bei einer großen Anzahl von Orders und Trades aggregiert `ReportSource` Daten automatisch, um die Berichtsgröße zu reduzieren:

```csharp
// Automatischer Aggregationsschwellenwert (Standardwert ist 10000)
source.MaxOrdersBeforeAggregation = 5000;
source.MaxTradesBeforeAggregation = 5000;

// Gruppierungsintervall (Standardwert ist 1 Stunde)
source.AggregationInterval = TimeSpan.FromMinutes(30);

// Manuelle Aggregation
source.AggregateOrders(TimeSpan.FromHours(1));
source.AggregateTrades(TimeSpan.FromHours(1));
```

Während der Aggregation werden Orders und Trades nach Zeitintervall, Instrument und Richtung gruppiert. Volumina werden summiert, Preise als gewichtete Durchschnitte berechnet.

## PositionLifecycleTracker

`PositionLifecycleTracker` verfolgt den Lebenszyklus von Positionen und erzeugt Round-Trips, also Datensätze über Positionsöffnung und -schließung.

Ein Round-Trip wird aufgezeichnet, wenn:
- Eine Position vollständig geschlossen wird, also der Wert null wird
- Eine Positionsumkehr stattfindet, also ein Vorzeichenwechsel

In der Klasse `Strategy` ist der Tracker automatisch integriert: abgeschlossene Round-Trips werden über das Ereignis `RoundTripClosed` zu `ReportSource` hinzugefügt.

```csharp
var tracker = new PositionLifecycleTracker();

// Ereignis beim Schließen eines Round-Trips
tracker.RoundTripClosed += roundTrip =>
{
    Console.WriteLine($"Position closed: {roundTrip.SecurityId}, " +
        $"Open: {roundTrip.OpenTime} at {roundTrip.OpenPrice}, " +
        $"Close: {roundTrip.CloseTime} at {roundTrip.ClosePrice}, " +
        $"Max volume: {roundTrip.MaxPosition}");
};

// Positionenaktualisierung verarbeiten
tracker.ProcessPosition(position);

// Zugriff auf die Round-Trip-Historie
IReadOnlyList<ReportPosition> history = tracker.History;
```

## Berichtsgeneratoren

Die folgenden Generatoren sind verfügbar:

| Generator | Format | Beschreibung |
|-----------|--------|--------------|
| `CsvReportGenerator` | CSV | Textformat mit Trennzeichen |
| `JsonReportGenerator` | JSON | Strukturiertes JSON |
| `XmlReportGenerator` | XML | XML-Format |
| `ExcelReportGenerator` | Excel | Excel-Format (erfordert `IExcelWorkerProvider`) |

Alle Generatoren erben von `BaseReportGenerator` und unterstützen die Konfiguration der enthaltenen Abschnitte:

```csharp
var generator = new CsvReportGenerator();

// Berichtsabschnitte konfigurieren
generator.IncludeOrders = true;
generator.IncludeTrades = true;
generator.IncludePositions = true;
generator.Encoding = Encoding.UTF8;
```

## Bericht aus einer Strategie erzeugen

Da `Strategy` `IReportSource` implementiert, kann ein Bericht direkt erzeugt werden:

```csharp
// Die Strategie selbst ist die Datenquelle
var generator = new JsonReportGenerator();

using var stream = File.Create("report.json");
await generator.Generate(strategy, stream, CancellationToken.None);
```

Für eine separate Datenquelle:

```csharp
var source = new ReportSource();
source.Name = strategy.Name;
source.PnL = strategy.PnL;
source.TotalWorkingTime = strategy.TotalWorkingTime;

// Positionen aus dem Tracker hinzufügen
source.AddPositions(tracker.History);

var generator = new CsvReportGenerator();
using var stream = File.Create("report.csv");
await generator.Generate(source, stream, CancellationToken.None);
```

## Beispiel: Strategie mit Berichtserstellung beim Stoppen

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

        // Handelslogik...
    }

    protected override void OnStopped()
    {
        // Bericht erzeugen, wenn die Strategie stoppt
        var generator = new CsvReportGenerator();

        using var stream = File.Create($"report_{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.csv");
        generator.Generate(this, stream, CancellationToken.None).AsTask().Wait();

        base.OnStopped();
    }
}
```

In diesem Beispiel erstellt die Strategie beim Stoppen automatisch einen CSV-Bericht. Der Bericht enthält Strategieparameter, Statistiken, Orders, Trades und Positions-Round-Trips.

