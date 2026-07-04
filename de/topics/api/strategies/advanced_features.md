# Erweiterte Strategiefunktionen

## Überblick

Die Klasse `Strategy` stellt mehrere zusätzliche Eigenschaften zur Feinabstimmung des Verhaltens bereit: automatische Orderkommentare, Handelszeitplan, risikofreier Zinssatz für Statistiken, Datenquelle für Indikatoren und Verwaltung historischer Zeiträume.

## CommentMode - Orderkommentare

Die Eigenschaft `CommentMode` steuert die automatische Befüllung des Felds `Order.Comment` für alle von der Strategie gesendeten Orders. Dadurch lässt sich erkennen, welche Strategie eine Order erstellt hat; das ist besonders nützlich, wenn mehrere Strategien gleichzeitig auf demselben Konto laufen.

### Enumeration StrategyCommentModes

| Wert | Beschreibung |
|------|--------------|
| `Disabled` | Der Kommentar wird nicht automatisch befüllt. Standardwert. |
| `Id` | Der Kommentar wird auf `Strategy.Id` gesetzt (eindeutiger GUID-Bezeichner). |
| `Name` | Der Kommentar wird auf `Strategy.Name` gesetzt (Strategiename). |

### Beispiel

```csharp
public class CommentStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Alle Orders werden mit dem Strategienamen markiert
        CommentMode = StrategyCommentModes.Name;

        // Oder mit dem Bezeichner für exakte Zuordnung
        // CommentMode = StrategyCommentModes.Id;
    }
}
```

Mit dem Wert `Name` und dem Strategienamen "SMA Crossover" erhält jede Order den Kommentar "SMA Crossover", sodass Sie die Orders dieser Strategie im Handelsjournal filtern können.

## WorkingTime - Arbeitszeitplan

Die Eigenschaft `WorkingTime` legt den Zeitplan fest, während dessen die Strategie aktiv ist. Außerhalb der angegebenen Zeitintervalle kann die Strategie ihre Aktivität automatisch einschränken.

```csharp
public class ScheduledStrategy : Strategy
{
    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Arbeitszeit konfigurieren
        WorkingTime = new WorkingTime
        {
            Periods = new List<WorkingTimePeriod>
            {
                new WorkingTimePeriod
                {
                    Till = DateTime.MaxValue,
                    Times = new List<Range<TimeSpan>>
                    {
                        // Handel von 10:00 bis 18:00
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

Die schreibgeschützte Eigenschaft `TotalWorkingTime` zeigt die gesamte Arbeitszeit der Strategie seit ihrem Start. Sie wird beim Stoppen und Neustarten der Strategie automatisch berechnet.

## RiskFreeRate - Risikofreier Zinssatz

Die Eigenschaft `RiskFreeRate` legt den jährlichen risikofreien Zinssatz fest, der in statistischen Berechnungen verwendet wird - vor allem für Sharpe Ratio und Sortino Ratio.

```csharp
var strategy = new MyStrategy();

// Risikofreier Zinssatz von 5 % p. a.
strategy.RiskFreeRate = 0.05m;
```

Der Wert wird automatisch an alle Statistikparameter übergeben, die `IRiskFreeRateStatisticParameter` implementieren, wenn der Statistikmanager der Strategie initialisiert wird.

## IndicatorSource - Indikatordatenquelle

Die Eigenschaft `IndicatorSource` setzt den Standardwert für die Eigenschaft `IIndicator.Source` aller Strategieindikatoren, für die keine Quelle explizit angegeben wurde. Sie definiert, welches Feld aus `Level1Fields` als Eingabedaten für Indikatoren verwendet wird.

```csharp
var strategy = new MyStrategy();

// Alle Indikatoren verwenden standardmäßig den Preis des letzten Trades
strategy.IndicatorSource = Level1Fields.LastTradePrice;

// Oder den Durchschnittspreis
// strategy.IndicatorSource = Level1Fields.AveragePrice;
```

Wenn die Eigenschaft `null` ist (Standardwert), verwenden Indikatoren ihre eigene Datenquelle.

## HistoryCalculated - Berechneter historischer Zeitraum

Die virtuelle Eigenschaft `HistoryCalculated` ermöglicht einer Strategie, den erforderlichen historischen Datenzeitraum für das Aufwärmen von Indikatoren programmatisch zu bestimmen. Sie gibt `TimeSpan?` zurück - die Dauer des historischen Zeitraums oder `null`, wenn kein Zeitraum angegeben ist.

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

    // Automatische Berechnung des erforderlichen historischen Zeitraums
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(LongPeriod * 2);
}
```

`HistoryCalculated` ist die im Code berechnete Variante der Eigenschaft `HistorySize`. Der Unterschied besteht darin, dass `HistorySize` vom Benutzer als Strategieparameter gesetzt wird, während `HistoryCalculated` programmatisch auf Basis von Strategieparametern berechnet wird (zum Beispiel Indikatorperioden).

## Beispiel: Strategie mit allen erweiterten Einstellungen

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

    // Automatische Berechnung des historischen Zeitraums
    protected override TimeSpan? HistoryCalculated
        => TimeSpan.FromDays(SmaPeriod * 2);

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Orderkommentare - Strategiename
        CommentMode = StrategyCommentModes.Name;

        // Risikofreier Zinssatz für die Sharpe-Berechnung
        RiskFreeRate = 0.05m;

        // Datenquelle für Indikatoren
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

        // Handelslogik...
    }
}
```

In diesem Beispiel verwendet die Strategie alle beschriebenen Funktionen: Sie kommentiert Orders automatisch, setzt den risikofreien Zinssatz für Statistiken, legt die Datenquelle für Indikatoren fest und berechnet den erforderlichen historischen Zeitraum.
