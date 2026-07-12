# Timer-System

## Überblick

Strategien in StockSharp unterstützen ein integriertes Timer-System, mit dem Aktionen in festgelegten Zeitintervallen ausgeführt werden können. Timer basieren auf dem Mechanismus `WhenIntervalElapsed` des Connectors und funktionieren sowohl im Live-Handel als auch bei Rücktests korrekt, wobei im Rücktest die virtuelle Zeit des Emulators verwendet wird.

Timer sind nützlich für:

- Regelmäßige Prüfungen der Marktbedingungen
- Portfolio-Rebalancing in festen Intervallen
- Schließen von Positionen nach Zeit
- Überwachung des Strategiezustands

## ITimerHandler-Schnittstelle

Der Timer wird über die Schnittstelle `ITimerHandler` verwaltet, die die folgenden Member bereitstellt:

| Member | Beschreibung |
|--------|--------------|
| `Start()` | Startet den Timer. Gibt `ITimerHandler` für Method Chaining zurück |
| `Stop()` | Stoppt den Timer. Gibt `ITimerHandler` für Method Chaining zurück |
| `Interval` | Auslöseintervall des Timers (lesbar und schreibbar) |
| `IsStarted` | Gibt an, ob der Timer läuft |
| `Dispose()` | Gibt Ressourcen frei und stoppt den Timer |

## Methoden zur Timer-Erstellung

### CreateTimer

Erstellt einen Timer, startet ihn aber nicht. Der Start erfolgt über einen separaten Aufruf von `Start()`:

```csharp
ITimerHandler CreateTimer(TimeSpan interval, Action callback);
```

### StartTimer

Erstellt und startet einen Timer sofort. Entspricht `CreateTimer(...).Start()`:

```csharp
ITimerHandler StartTimer(TimeSpan interval, Action callback);
```

Das Intervall muss ein positiver Wert sein (`> TimeSpan.Zero`), andernfalls wird eine Ausnahme ausgelöst.

## Timer-Verwaltung

Ein Timer kann gestoppt und neu gestartet werden:

```csharp
var timer = CreateTimer(TimeSpan.FromMinutes(1), MyCallback);

// Starten
timer.Start();

// Stoppen
timer.Stop();

// Intervall ändern
timer.Interval = TimeSpan.FromMinutes(5);

// Erneut starten
timer.Start();

// Ressourcen freigeben
timer.Dispose();
```

## Verwendungsbeispiel

```csharp
public class TimerStrategy : Strategy
{
    private ITimerHandler _checkTimer;
    private ITimerHandler _closeTimer;

    private readonly StrategyParam<TimeSpan> _checkInterval;
    private readonly StrategyParam<TimeSpan> _maxHoldTime;

    public TimeSpan CheckInterval
    {
        get => _checkInterval.Value;
        set => _checkInterval.Value = value;
    }

    public TimeSpan MaxHoldTime
    {
        get => _maxHoldTime.Value;
        set => _maxHoldTime.Value = value;
    }

    public TimerStrategy()
    {
        _checkInterval = Param(nameof(CheckInterval), TimeSpan.FromMinutes(1));
        _maxHoldTime = Param(nameof(MaxHoldTime), TimeSpan.FromHours(4));
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Timer für regelmäßige Prüfungen der Marktbedingungen
        _checkTimer = StartTimer(CheckInterval, OnCheckTimer);

        // Timer für erzwungenes Schließen von Positionen (erstellt, aber nicht gestartet)
        _closeTimer = CreateTimer(MaxHoldTime, OnCloseTimer);

        var subscription = SubscribeCandles(TimeSpan.FromMinutes(5));

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void OnCheckTimer()
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        this.AddInfoLog("Prüfe Marktbedingungen um {0}", CurrentTime);

        // Regelmäßige Prüfung des Positionszustands
        if (Position != 0)
        {
            this.AddInfoLog("Aktuelle Position: {0}", Position);
        }
    }

    private void OnCloseTimer()
    {
        if (Position != 0)
        {
            this.AddInfoLog("Haltezeit der Position abgelaufen, schließe");
            ClosePosition();

            // Close-Timer nach Auslösung stoppen
            _closeTimer.Stop();
        }
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice && Position == 0)
        {
            BuyMarket();

            // Timer für erzwungenes Schließen starten
            _closeTimer.Start();
        }
    }
}
```

In diesem Beispiel werden zwei Timer verwendet: einer für die regelmäßige Zustandsprüfung (`StartTimer` -- erstellt und sofort gestartet) und ein weiterer zur Begrenzung der Positionshaltedauer (`CreateTimer` -- erstellt, aber erst beim Einstieg in eine Position gestartet).

