# Verwaltung der Zielposition

## Überblick

Das System zur Verwaltung der Zielposition ermöglicht es einer Strategie, die gewünschte Positionsgröße deklarativ anzugeben, während die Plattform automatisch die erforderlichen Orders platziert, um dieses Niveau zu erreichen. Statt Volumen und Richtung eines Trades manuell zu berechnen, rufen Sie einfach `SetTargetPosition(10)` auf -- und der Manager bestimmt, ob gekauft oder verkauft werden muss und mit welchem Volumen.

Die Schlüsselkomponente ist die Klasse `PositionTargetManager`, die automatisch:

- Die Differenz zwischen aktueller und Zielposition berechnet
- Richtung und Volumen der Order bestimmt
- Orderausführung, Stornierung und Fehler behandelt
- Wiederholungsversuche bei Fehlern unterstützt

## Strategiemethoden

### SetTargetPosition

Setzt die Zielposition. Zwei Aufrufvarianten sind verfügbar:

```csharp
// Für das Hauptinstrument und Portfolio der Strategie
SetTargetPosition(decimal target);

// Für ein beliebiges Instrument und Portfolio
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

Wenn `target` größer als die aktuelle Position ist, platziert der Manager eine Kauforder. Wenn es kleiner ist, eine Verkaufsorder. Wenn die Position unter Berücksichtigung von `PositionTolerance` bereits dem Ziel entspricht, wird keine Aktion ausgeführt.

### CancelTargetPosition

Storniert eine zuvor gesetzte Zielposition und stoppt alle zugehörigen aktiven Orders:

```csharp
// Für das Hauptinstrument und Portfolio der Strategie
CancelTargetPosition();

// Für ein beliebiges Instrument und Portfolio
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

Gibt den aktuellen Wert der Zielposition zurück oder `null`, wenn kein Ziel gesetzt ist:

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## TargetPositionManager-Eigenschaft

Die Eigenschaft `TargetPositionManager` bietet direkten Zugriff auf das Objekt `PositionTargetManager` zur Feinabstimmung:

```csharp
// Maximale Anzahl von Wiederholungsversuchen bei Orderfehlern (Standardwert ist 3)
TargetPositionManager.MaxRetries = 5;

// Toleranz zur Bestimmung, ob die Zielposition erreicht ist
TargetPositionManager.PositionTolerance = 0.01m;

// Ordertyp (Standardwert ist Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

Der Manager erzeugt die folgenden Ereignisse:

- `TargetReached` -- Zielposition wurde erreicht
- `Error` -- Bei der Orderausführung ist ein Fehler aufgetreten
- `OrderRegistered` -- Der Manager hat eine Order registriert

## TargetAlgoFactory-Eigenschaft

Die Eigenschaft `TargetAlgoFactory` erlaubt das Festlegen einer Factory für Algorithmen zur Positionsänderung. Standardmäßig wird `MarketOrderAlgo` verwendet, das Market-Orders erstellt:

```csharp
// Benutzerdefinierten Algorithmus statt Market-Orders verwenden
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## Verwendungsbeispiel

```csharp
public class TargetPositionStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TargetPositionStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Zielpositionsmanager konfigurieren
        TargetPositionManager.MaxRetries = 5;
        TargetPositionManager.TargetReached += (sec, pf) =>
        {
            this.AddInfoLog("Target position reached: {0}, {1}", sec, pf);
        };

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.OpenPrice < candle.ClosePrice)
        {
            // Bullische Kerze -- Zielposition für Kauf setzen
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Bärische Kerze -- Zielposition für Verkauf setzen
            SetTargetPosition(-Volume);
        }
    }
}
```

In diesem Beispiel befasst sich die Strategie nicht mit manuellen Berechnungen von Volumen und Richtung. Sie deklariert einfach die gewünschte Positionsgröße, und `PositionTargetManager` übernimmt die gesamte Orderplatzierung.

