# Target Position Management

## Overview

The target position management system allows a strategy to declaratively specify the desired position size, while the platform automatically places the necessary orders to reach that level. Instead of manually calculating the volume and direction of a trade, you simply call `SetTargetPosition(10)` -- and the manager will determine whether to buy or sell, and in what volume.

The key component is the `PositionTargetManager` class, which automatically:

- Calculates the difference between the current and target position
- Determines the direction and volume of the order
- Handles order execution, cancellation, and errors
- Supports retry attempts on failures

## Strategy Methods

### SetTargetPosition

Sets the target position. Two calling variants are available:

```csharp
// For the strategy's main security and portfolio
SetTargetPosition(decimal target);

// For an arbitrary security and portfolio
SetTargetPosition(Security security, Portfolio portfolio, decimal target);
```

When `target` is greater than the current position, the manager will place a buy order. When less -- a sell order. If the position already equals the target (taking into account `PositionTolerance`), no action is taken.

### CancelTargetPosition

Cancels a previously set target position and stops all related active orders:

```csharp
// For the strategy's main security and portfolio
CancelTargetPosition();

// For an arbitrary security and portfolio
CancelTargetPosition(Security security, Portfolio portfolio);
```

### GetTargetPosition

Returns the current target position value, or `null` if no target is set:

```csharp
decimal? target = GetTargetPosition();
decimal? target = GetTargetPosition(security, portfolio);
```

## TargetPositionManager Property

The `TargetPositionManager` property provides direct access to the `PositionTargetManager` object for fine-tuning:

```csharp
// Maximum number of retry attempts on order error (default is 3)
TargetPositionManager.MaxRetries = 5;

// Tolerance for determining whether the target position is reached
TargetPositionManager.PositionTolerance = 0.01m;

// Order type (default is Market)
TargetPositionManager.OrderType = OrderTypes.Market;
```

The manager generates the following events:

- `TargetReached` -- target position has been reached
- `Error` -- an error occurred during order execution
- `OrderRegistered` -- the manager has registered an order

## TargetAlgoFactory Property

The `TargetAlgoFactory` property allows setting a factory for position change algorithms. By default, `MarketOrderAlgo` is used, which creates market orders:

```csharp
// Use a custom algorithm instead of market orders
TargetAlgoFactory = (side, volume) => new MyCustomAlgo(side, volume);
```

## Usage Example

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

        // Configure the target position manager
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
            // Bullish candle -- set target position for buying
            SetTargetPosition(Volume);
        }
        else if (candle.OpenPrice > candle.ClosePrice)
        {
            // Bearish candle -- set target position for selling
            SetTargetPosition(-Volume);
        }
    }
}
```

In this example, the strategy does not deal with manual volume and direction calculations. It simply declares the desired position size, and `PositionTargetManager` handles all the order placement work.
