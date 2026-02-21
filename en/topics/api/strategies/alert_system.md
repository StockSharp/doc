# Alert System

## Overview

Strategies in StockSharp have a built-in alert system that allows sending notifications of various types: popup windows, sound signals, log entries, and Telegram messages. Alerts are useful for informing the trader about important events -- position entries, level breaches, errors, and other trading signals.

During backtesting, alerts other than the `Log` type are automatically skipped to avoid interference during testing.

## Alert Types

The `AlertNotifications` enumeration defines the available types:

| Type | Description |
|------|-------------|
| `Sound` | Sound signal |
| `Popup` | Popup window |
| `Log` | Log file entry |
| `Telegram` | Telegram message |

## Methods

### Alert

Base method for sending an alert with a specified type, caption, and message:

```csharp
// With caption and message
Alert(AlertNotifications type, string caption, string message);

// With automatic caption (uses the strategy name)
Alert(AlertNotifications type, string message);
```

### AlertPopup

Sends a popup notification. The caption is the strategy name:

```csharp
AlertPopup(string message);
```

### AlertSound

Sends a sound notification:

```csharp
AlertSound(string message);
```

### AlertLog

Sends a notification to the log. This type works during backtesting as well:

```csharp
AlertLog(string message);
```

## Configuring the Alert Service

For alerts to work, the `IAlertNotificationService` service must be registered in the strategy's environment. This is done via the extension method:

```csharp
strategy.SetAlertService(alertService);
```

The current service can be retrieved via:

```csharp
var service = strategy.GetAlertService();
```

In graphical applications (Designer, terminal), the service is usually registered automatically.

## Usage Example

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

        // Alert about strategy start
        AlertLog("Strategy started, tracked level: " + PriceLevel);
    }

    private void ProcessCandle(ICandleMessage candle)
    {
        if (!IsFormedAndOnlineAndAllowTrading())
            return;

        // Price crossed the level from below upward
        if (candle.OpenPrice < PriceLevel && candle.ClosePrice >= PriceLevel)
        {
            AlertPopup("Price crossed level " + PriceLevel + " upward!");
            AlertSound("Level breakout!");
            BuyMarket();
        }

        // Price crossed the level from above downward
        if (candle.OpenPrice > PriceLevel && candle.ClosePrice <= PriceLevel)
        {
            Alert(AlertNotifications.Telegram, "Trading signal",
                "Price broke level " + PriceLevel + " downward");
            SellMarket();
        }
    }
}
```

In this example, the strategy uses different alert types for different situations: `AlertPopup` and `AlertSound` for immediately capturing the trader's attention, and `Alert` with the `Telegram` type for remote notification.
