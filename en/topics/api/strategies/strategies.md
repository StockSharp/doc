# Strategies

## Overview

The `Strategy` class is the base class for creating trading strategies in StockSharp. It provides a complete set of tools for subscribing to market data, managing orders and positions, calculating statistics, and generating reports.

Key capabilities of the `Strategy` class:

- Subscribing to candles, order books, ticks, and other market data
- Placing, modifying, and canceling orders
- Target position management
- PnL, commission, and statistics calculation
- Risk management
- Timer and rule system
- Alerts
- Report generation

> [!WARNING]
> Child strategies functionality (`ChildStrategies`) has been declared obsolete and is no longer supported. The `ChildStrategies` property is marked with the `[Obsolete("Child strategies no longer supported.")]` attribute. If your code uses child strategies, it is recommended to refactor -- run each strategy as an independent instance.

## Documentation Sections

- [Target Position Management](target_position_management.md) -- declarative position size management via `SetTargetPosition`
- [Trading Modes](trading_modes.md) -- restricting trading activity via `StrategyTradingModes`
- [Alert System](alert_system.md) -- sending notifications (popup, sound, log, Telegram)
- [Timer System](timer_system.md) -- periodic action execution
- [Risk Management](risk_management.md) -- risk management rules
- [High-Level Subscriptions](high_level_subscriptions.md) -- simplified market data subscriptions
- [Strategy Reports](reporting.md) -- generating trade result reports
- [Advanced Features](advanced_features.md) -- order comments, schedules, risk-free rate, indicator source

## Minimal Strategy

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

        // Trading logic
    }
}
```

## Strategy Lifecycle

1. **Creation** -- constructor, declaring parameters via `Param<T>`.
2. **Configuration** -- setting `Security`, `Portfolio`, `Connector`, and parameters.
3. **Start** -- calling `Start()`, transition to `ProcessStates.Started` state, invocation of `OnStarted2(DateTime)`.
4. **Running** -- processing market data, placing orders.
5. **Stop** -- calling `Stop()`, transition through `ProcessStates.Stopping` to `ProcessStates.Stopped`, invocation of `OnStopped()`.
