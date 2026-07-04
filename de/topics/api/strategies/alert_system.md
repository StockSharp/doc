# Alert-System

## Überblick

Strategien in StockSharp besitzen ein integriertes Alert-System, mit dem Benachrichtigungen verschiedener Typen gesendet werden können: Popup-Fenster, Tonsignale, Logeinträge und Telegram-Nachrichten. Alerts sind nützlich, um den Trader über wichtige Ereignisse zu informieren - Positionseinstiege, Level-Durchbrüche, Fehler und andere Handelssignale.

Während des Backtestings werden Alerts außer dem Typ `Log` automatisch übersprungen, damit sie den Testlauf nicht stören.

## Alert-Typen

Die Enumeration `AlertNotifications` definiert die verfügbaren Typen:

| Typ | Beschreibung |
|-----|--------------|
| `Sound` | Tonsignal |
| `Popup` | Popup-Fenster |
| `Log` | Logdateieintrag |
| `Telegram` | Telegram-Nachricht |

## Methoden

### Alert

Basismethode zum Senden eines Alerts mit angegebenem Typ, Titel und Nachricht:

```csharp
// Mit Titel und Nachricht
Alert(AlertNotifications type, string caption, string message);

// Mit automatischem Titel (verwendet den Strategienamen)
Alert(AlertNotifications type, string message);
```

### AlertPopup

Sendet eine Popup-Benachrichtigung. Der Titel ist der Strategiename:

```csharp
AlertPopup(string message);
```

### AlertSound

Sendet eine Tonbenachrichtigung:

```csharp
AlertSound(string message);
```

### AlertLog

Sendet eine Benachrichtigung in das Log. Dieser Typ funktioniert auch während des Backtestings:

```csharp
AlertLog(string message);
```

## Alert-Service konfigurieren

Damit Alerts funktionieren, muss der Service `IAlertNotificationService` in der Umgebung der Strategie registriert sein. Dies erfolgt über die Erweiterungsmethode:

```csharp
strategy.SetAlertService(alertService);
```

Der aktuelle Service kann wie folgt abgerufen werden:

```csharp
var service = strategy.GetAlertService();
```

In grafischen Anwendungen (Designer, Terminal) wird der Service normalerweise automatisch registriert.

## Verwendungsbeispiel

```csharp
public class AlertStrategy : Strategy
{
    private readonly StrategyParam<DataType> _candleType;
    private readonly StrategyParam<decimal> _priceLevel;

    public DataType CandleType
    {
        get => _candleType.Value;
        set => _candleType.Value = value;
    }

    public decimal PriceLevel
    {
        get => _priceLevel.Value;
        set => _priceLevel.Value = value;
    }

    public AlertStrategy()
    {
        _candleType = Param(nameof(CandleType), TimeSpan.FromMinutes(5).TimeFrame());
        _priceLevel = Param(nameof(PriceLevel), 100m);
    }

    protected override void OnStarted2(DateTime time)
    {
        base.OnStarted2(time);

        var subscription = SubscribeCandles(CandleType);

        subscription
            .Bind(ProcessCandle)
            .Start();

        // Alert über Strategiestart
        AlertLog("Strategy started, tracked level: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Preis hat das Level von unten nach oben gekreuzt
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("Price crossed level " + PriceLevel + " upward!");
            AlertSound("Level breakout!");
            BuyMarket();
        }

        // Preis hat das Level von oben nach unten gekreuzt
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Trading signal",
                "Price broke level " + PriceLevel + " downward");
            SellMarket();
        }
    }
}
```

In diesem Beispiel verwendet die Strategie verschiedene Alert-Typen für unterschiedliche Situationen: `AlertPopup` und `AlertSound`, um sofort die Aufmerksamkeit des Traders zu gewinnen, sowie `Alert` mit dem Typ `Telegram` für Remote-Benachrichtigungen.
