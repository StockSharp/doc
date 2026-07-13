# Handelsmodi von Strategien

## Überblick

Die Eigenschaft `TradingMode` ermöglicht es, die Handelsaktivität einer Strategie einzuschränken, ohne sie vollständig zu stoppen. Dies ist für das Risikomanagement nützlich, beispielsweise um das Öffnen neuer Positionen zu verbieten und nur das Schließen bestehender Positionen zu erlauben oder die Orderübermittlung vollständig zu blockieren.

Der Modus wird über die Enumeration `StrategyTradingModes` gesetzt und kann geändert werden, während die Strategie läuft.

## StrategyTradingModes-Enumeration

| Wert | Beschreibung |
|------|--------------|
| `Full` | Voller Handelszugriff. Keine Einschränkungen für Orders. Standardwert. |
| `Disabled` | Handel ist vollständig verboten. Alle Versuche zur Orderplatzierung werden abgelehnt. |
| `CancelOrdersOnly` | Nur Orderstornierung ist erlaubt. Neue Orders und Änderungen bestehender Orders sind verboten. |
| `ReducePositionOnly` | Nur Orders, die die aktuelle Position reduzieren, sind erlaubt. Das Öffnen neuer Positionen und das Erhöhen bestehender Positionen sind verboten. |
| `LongOnly` | Nur Long-Positionen sind erlaubt. Verkaufen ist nur zum Schließen einer bestehenden Long-Position erlaubt; das Verkaufsvolumen darf die aktuelle Position nicht überschreiten. Das Öffnen von Short-Positionen ist verboten. |

## Modus setzen

```csharp
// Beim Erstellen der Strategie
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Dynamische Änderung während der Ausführung
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## Logik der Modusprüfung

Beim Versuch, eine Order zu registrieren, prüft die Strategie den aktuellen Modus:

- **`Disabled`** -- die Order wird mit dem Grund "Handel ist verboten" abgelehnt.
- **`ReducePositionOnly`** -- die Order wird abgelehnt, wenn die aktuelle Position null ist, wenn die Orderrichtung der Positionsrichtung entspricht oder wenn das Ordervolumen den absoluten Wert der Position überschreitet.
- **`LongOnly`** -- eine Verkaufsorder wird abgelehnt, wenn die aktuelle Position nicht positiv ist oder wenn das Verkaufsvolumen die aktuelle Position überschreitet.
- **`Full`** -- keine Einschränkungen.
- **`CancelOrdersOnly`** -- nur Orderstornierung ist erlaubt.

## Methode IsFormedAndOnlineAndAllowTrading

Die Erweiterungsmethode `IsFormedAndOnlineAndAllowTrading` prüft, ob die Strategie gebildet ist (`IsFormed`), sich im Online-Zustand befindet (`IsOnline`) und der Handelsmodus die erforderliche Aktion erlaubt:

```csharp
// Berechtigung für vollen Handel prüfen (Standard)
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// Berechtigung nur für Orderstornierung prüfen
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// Berechtigung für Positionsreduktion prüfen
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

Berechtigungslogik beim Aufruf mit einem `required`-Parameter:

| Aktueller TradingMode \ erforderlich | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | ja | ja | ja |
| `Disabled` | nein | nein | nein |
| `CancelOrdersOnly` | nein | ja | nein |
| `ReducePositionOnly` | nein | ja | ja |
| `LongOnly` | nein | ja | ja |

## Verwendungsbeispiel

```csharp
public class TradingModeStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public TradingModeStrategy()
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
        // Prüfen, ob die Strategie für vollen Handel bereit ist
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        if (candle.ClosePrice > candle.OpenPrice)
        {
            BuyMarket(Volume);
        }
        else if (candle.ClosePrice < candle.OpenPrice)
        {
            SellMarket(Volume);
        }
    }
}

// Strategie mit Einschränkung starten -- nur Long-Positionen
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// Später -- in Positionsschließungsmodus wechseln
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Vollständige Handelssperre
strategy.TradingMode = StrategyTradingModes.Disabled;
```

In diesem Beispiel arbeitet die Strategie zunächst im Modus `LongOnly`, der nur Käufe und das Schließen von Long-Positionen erlaubt. Wenn sich die Marktbedingungen ändern, kann der Modus auf `ReducePositionOnly` für schrittweises Schließen von Positionen und anschließend auf `Disabled` für eine vollständige Unterbrechung der Handelsaktivität umgeschaltet werden.
