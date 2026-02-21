# Strategy Trading Modes

## Overview

The `TradingMode` property allows restricting a strategy's trading activity without stopping it completely. This is useful for risk management -- for example, prohibiting opening new positions while only allowing closing existing ones, or completely blocking order submission.

The mode is set using the `StrategyTradingModes` enumeration and can be changed while the strategy is running.

## StrategyTradingModes Enumeration

| Value | Description |
|-------|-------------|
| `Full` | Full trading access. No restrictions on orders. Default value. |
| `Disabled` | Trading is completely prohibited. All order placement attempts will be rejected. |
| `CancelOrdersOnly` | Only order cancellation is allowed. New orders and modification of existing ones are prohibited. |
| `ReducePositionOnly` | Only orders that reduce the current position are allowed. Opening new positions and increasing existing ones are prohibited. |
| `LongOnly` | Only long positions are allowed. Selling is only allowed to close an existing long position (sell volume cannot exceed the current position). Opening short positions is prohibited. |

## Setting the Mode

```csharp
// When creating the strategy
var strategy = new MyStrategy();
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Dynamic change during operation
strategy.TradingMode = StrategyTradingModes.Disabled;
```

## Mode Checking Logic

When attempting to register an order, the strategy checks the current mode:

- **`Disabled`** -- the order is rejected with the reason "trading is prohibited".
- **`ReducePositionOnly`** -- the order is rejected if the current position is zero, if the order direction matches the position direction, or if the order volume exceeds the absolute value of the position.
- **`LongOnly`** -- a sell order is rejected if the current position is non-positive or if the sell volume exceeds the current position.
- **`Full`** -- no restrictions.
- **`CancelOrdersOnly`** -- only order cancellation is allowed.

## IsFormedAndOnlineAndAllowTrading Method

The `IsFormedAndOnlineAndAllowTrading` extension method checks that the strategy is formed (`IsFormed`), is in an online state (`IsOnline`), and the trading mode allows the required action:

```csharp
// Check permission for full trading (default)
if (!IsFormedAndOnlineAndAllowTrading())
    return;

// Check permission for order cancellation only
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.CancelOrdersOnly))
    CancelActiveOrders();

// Check permission for position reduction
if (!IsFormedAndOnlineAndAllowTrading(StrategyTradingModes.ReducePositionOnly))
    return;
```

Permission logic when called with a `required` parameter:

| Current TradingMode \ required | `Full` | `CancelOrdersOnly` | `ReducePositionOnly` |
|-------------------------------|--------|---------------------|---------------------|
| `Full` | yes | yes | yes |
| `Disabled` | no | no | no |
| `CancelOrdersOnly` | no | yes | no |
| `ReducePositionOnly` | no | yes | yes |
| `LongOnly` | no | yes | yes |

## Usage Example

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
        // Check that the strategy is ready for full trading
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

// Start the strategy with a restriction -- long positions only
var strategy = new TradingModeStrategy();
strategy.TradingMode = StrategyTradingModes.LongOnly;
strategy.Start();

// Later -- switch to position closing mode
strategy.TradingMode = StrategyTradingModes.ReducePositionOnly;

// Full trading block
strategy.TradingMode = StrategyTradingModes.Disabled;
```

In this example, the strategy initially operates in `LongOnly` mode, which allows only purchases and closing of long positions. When market conditions change, the mode can be switched to `ReducePositionOnly` for gradual position closing, and then to `Disabled` for a complete halt of trading activity.
