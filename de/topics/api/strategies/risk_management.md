# Risikomanagement

## Überblick

Jede Strategie in StockSharp verfügt über einen integrierten `RiskManager`, der die automatische Kontrolle von Handelsrisiken ermöglicht. Wenn eine Regel ausgelöst wird, kann der Manager Positionen schließen, aktive Orders stornieren oder den Handel der Strategie vollständig stoppen.

Der Risikomanager verarbeitet alle Handelsmeldungen, die durch die Strategie laufen. Wenn Regelbedingungen erfüllt sind, führt er die festgelegte Aktion automatisch aus, ohne dass zusätzlicher Code in der Strategielogik erforderlich ist.

## Strategieeigenschaften

### RiskManager

Das Objekt `IRiskManager`, das die Liste der Regeln verwaltet. Es wird automatisch erstellt, wenn die Strategie initialisiert wird:

```csharp
// Zugriff auf den Risikomanager
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

Eine bequeme Eigenschaft zum Lesen und Setzen der Regelliste:

```csharp
// Aktuelle Regeln lesen
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Neue Regeln setzen
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## Auslöseaktionen

Die Enumeration `RiskActions` definiert die möglichen Aktionen:

| Aktion | Beschreibung |
|--------|--------------|
| `ClosePositions` | Alle offenen Positionen mit einer Market-Order schließen |
| `StopTrading` | Den Handel der Strategie blockieren |
| `CancelOrders` | Alle aktiven Orders stornieren |

Wenn eine Regel ausgelöst wird, protokolliert die Strategie eine Warnung mit dem Regelnamen, ihren Parametern und der ausgeführten Aktion.

## Verfügbare Regeln

### RiskPnLRule -- Gewinn-/Verlustkontrolle

Wird ausgelöst, wenn das angegebene P&L-Niveau erreicht ist. Ein positiver Wert kontrolliert Gewinn, ein negativer Wert kontrolliert Verlust:

```csharp
// Handel bei einem Verlust über 5000 stoppen
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- Kontrolle der Positionsgröße

Wird ausgelöst, wenn die angegebene Positionsgröße erreicht ist:

```csharp
// Orders stornieren, wenn Position >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- Kontrolle der Orderfrequenz

Wird ausgelöst, wenn die Anzahl der Orders innerhalb des angegebenen Intervalls das Limit überschreitet:

```csharp
// Stoppen bei mehr als 50 Orders pro Minute
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- Kontrolle des Ordervolumens

Wird ausgelöst, wenn das Volumen einer einzelnen Order überschritten wird.

### RiskOrderPriceRule -- Kontrolle des Orderpreises

Wird ausgelöst, wenn der Orderpreis die festgelegten Grenzen überschreitet.

### RiskTradeVolumeRule -- Kontrolle des Trade-Volumens

Wird ausgelöst, wenn das Volumen eines einzelnen Trades überschritten wird.

### RiskTradePriceRule -- Kontrolle des Trade-Preises

Wird ausgelöst, wenn der Trade-Preis die festgelegten Grenzen überschreitet.

### RiskTradeFreqRule -- Kontrolle der Trade-Frequenz

Wird ausgelöst, wenn die Anzahl der Trades innerhalb des Intervalls überschritten wird.

### RiskPositionTimeRule -- Kontrolle der Positionshaltedauer

Wird ausgelöst, wenn eine Position länger als die festgelegte Zeit gehalten wurde.

### RiskCommissionRule -- Kommissionskontrolle

Wird ausgelöst, wenn die Gesamtkommission überschritten wird.

### RiskSlippageRule -- Slippage-Kontrolle

Wird ausgelöst, wenn das Slippage-Niveau überschritten wird.

### RiskErrorRule -- Fehlerkontrolle

Wird ausgelöst, wenn sich eine bestimmte Anzahl von Fehlern angesammelt hat.

### RiskOrderErrorRule -- Kontrolle von Orderregistrierungsfehlern

Wird ausgelöst, wenn sich Orderregistrierungsfehler angesammelt haben.

## Verwendungsbeispiel

```csharp
public class RiskAwareStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public RiskAwareStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        // Risikomanagementregeln konfigurieren
        RiskRules = new IRiskRule[]
        {
            // Positionen bei einem Verlust über 10000 schließen
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Handel stoppen, wenn die Position 500 Kontrakte überschreitet
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Orders stornieren, wenn die Frequenz 100 pro Minute überschreitet
            new RiskOrderFreqRule
            {
                Count = 100,
                Interval = TimeSpan.FromMinutes(1),
                Action = RiskActions.CancelOrders
            },
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

        // Handelslogik
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

In diesem Beispiel werden Risikomanagementregeln beim Start der Strategie gesetzt. Wenn ein Verlust von 10000 erreicht wird, werden Positionen automatisch geschlossen. Wenn die Positionsgröße 500 Kontrakte überschreitet, wird der Handel blockiert. Wenn Orders zu häufig platziert werden, werden aktive Orders storniert. Alle diese Prüfungen werden automatisch ausgeführt, ohne zusätzlichen Code in der Methode `ProcessCandle`.

