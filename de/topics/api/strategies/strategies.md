# Strategien

## Überblick

Die Klasse `Strategy` ist die Basisklasse zum Erstellen von Handelsstrategien in StockSharp. Sie stellt einen vollständigen Werkzeugsatz zum Abonnieren von Marktdaten, Verwalten von Orders und Positionen, Berechnen von Statistiken und Erzeugen von Berichten bereit.

Wichtige Fähigkeiten der Klasse `Strategy`:

- Abonnieren von Kerzen, Orderbüchern, Ticks und anderen Marktdaten
- Platzieren, Ändern und Stornieren von Orders
- Verwaltung der Zielposition
- Berechnung von PnL, Kommission und Statistiken
- Risikomanagement
- Timer- und Regelsystem
- Alerts
- Berichtserstellung

> [!WARNING]
> Die Funktionalität für untergeordnete Strategien (`ChildStrategies`) wurde als veraltet deklariert und wird nicht mehr unterstützt. Die Eigenschaft `ChildStrategies` ist mit dem Attribut `[Obsolete("Child strategies no longer supported.")]` markiert. Wenn Ihr Code untergeordnete Strategien verwendet, wird ein Refactoring empfohlen: Führen Sie jede Strategie als unabhängige Instanz aus.

## Dokumentationsabschnitte

- [Verwaltung der Zielposition](target_position_management.md) -- deklarative Verwaltung der Positionsgröße über `SetTargetPosition`
- [Handelsmodi](trading_modes.md) -- Einschränkung der Handelsaktivität über `StrategyTradingModes`
- [Alert-System](alert_system.md) -- Senden von Benachrichtigungen (Popup, Sound, Log, Telegram)
- [Timer-System](timer_system.md) -- Ausführung periodischer Aktionen
- [Risikomanagement](risk_management.md) -- Regeln für das Risikomanagement
- [High-Level-Abonnements](high_level_subscriptions.md) -- vereinfachte Marktdatenabonnements
- [Strategieberichte](reporting.md) -- Erzeugen von Berichten über Handelsergebnisse
- [Erweiterte Funktionen](advanced_features.md) -- Orderkommentare, Zeitpläne, risikofreier Zinssatz, Indikatorquelle

## Minimale Strategie

```csharp
public class MyStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public MyStrategy()
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

        // Handelslogik
    }
}
```

## Lebenszyklus einer Strategie

1. **Erstellung** -- Konstruktor, Deklaration von Parametern über `Param<T>`.
2. **Konfiguration** -- Setzen von `Security`, `Portfolio`, `Connector` und Parametern.
3. **Start** -- Aufruf von `Start()`, Übergang in den Zustand `ProcessStates.Started`, Aufruf von `OnStarted2(DateTime)`.
4. **Ausführung** -- Verarbeitung von Marktdaten, Platzierung von Orders.
5. **Stopp** -- Aufruf von `Stop()`, Übergang über `ProcessStates.Stopping` zu `ProcessStates.Stopped`, Aufruf von `OnStopped()`.

