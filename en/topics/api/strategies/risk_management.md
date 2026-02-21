# Risk Management

## Overview

Every strategy in StockSharp has a built-in `RiskManager` that allows automatic control of trading risks. When a rule is triggered, the manager can close positions, cancel active orders, or completely stop the strategy's trading.

The risk manager processes all trading messages passing through the strategy, and when rule conditions are met, it automatically performs the specified action without any additional code in the strategy logic.

## Strategy Properties

### RiskManager

The `IRiskManager` object that manages the list of rules. It is created automatically when the strategy is initialized:

```csharp
// Access the risk manager
IRiskManager manager = strategy.RiskManager;
```

### RiskRules

A convenient property for reading and setting the list of rules:

```csharp
// Read current rules
IEnumerable<IRiskRule> rules = strategy.RiskRules;

// Set new rules
strategy.RiskRules = new IRiskRule[]
{
    new RiskPnLRule { PnL = -1000m, Action = RiskActions.StopTrading },
    new RiskPositionSizeRule { Position = 100m, Action = RiskActions.ClosePositions },
};
```

## Trigger Actions

The `RiskActions` enumeration defines the possible actions:

| Action | Description |
|--------|-------------|
| `ClosePositions` | Close all open positions with a market order |
| `StopTrading` | Block the strategy's trading |
| `CancelOrders` | Cancel all active orders |

When a rule is triggered, the strategy logs a warning with the rule name, its parameters, and the action performed.

## Available Rules

### RiskPnLRule -- profit/loss control

Triggers when the specified P&L level is reached. A positive value controls profit, a negative value controls loss:

```csharp
// Stop trading on a loss exceeding 5000
new RiskPnLRule
{
    PnL = -5000m,
    Action = RiskActions.StopTrading
}
```

### RiskPositionSizeRule -- position size control

Triggers when the specified position size is reached:

```csharp
// Cancel orders when position >= 100
new RiskPositionSizeRule
{
    Position = 100m,
    Action = RiskActions.CancelOrders
}
```

### RiskOrderFreqRule -- order frequency control

Triggers when the number of orders within the specified interval exceeds the limit:

```csharp
// Stop on more than 50 orders per minute
new RiskOrderFreqRule
{
    Count = 50,
    Interval = TimeSpan.FromMinutes(1),
    Action = RiskActions.StopTrading
}
```

### RiskOrderVolumeRule -- order volume control

Triggers when the volume of a single order is exceeded.

### RiskOrderPriceRule -- order price control

Triggers when the order price goes beyond the established boundaries.

### RiskTradeVolumeRule -- trade volume control

Triggers when the volume of a single trade is exceeded.

### RiskTradePriceRule -- trade price control

Triggers when the trade price goes beyond the established boundaries.

### RiskTradeFreqRule -- trade frequency control

Triggers when the number of trades within the interval is exceeded.

### RiskPositionTimeRule -- position hold time control

Triggers when a position has been held longer than the established time.

### RiskCommissionRule -- commission control

Triggers when the total commission is exceeded.

### RiskSlippageRule -- slippage control

Triggers when the slippage level is exceeded.

### RiskErrorRule -- error control

Triggers when a certain number of errors have accumulated.

### RiskOrderErrorRule -- order registration error control

Triggers when order registration errors have accumulated.

## Usage Example

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

        // Configure risk management rules
        RiskRules = new IRiskRule[]
        {
            // Close positions on a loss exceeding 10000
            new RiskPnLRule
            {
                PnL = -10000m,
                Action = RiskActions.ClosePositions
            },

            // Stop trading when position exceeds 500 contracts
            new RiskPositionSizeRule
            {
                Position = 500m,
                Action = RiskActions.StopTrading
            },

            // Cancel orders when frequency exceeds 100 per minute
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

        // Trading logic
        if (candle.OpenPrice < candle.ClosePrice && Position <= 0)
            BuyMarket(Volume + Math.Abs(Position));
        else if (candle.OpenPrice > candle.ClosePrice && Position >= 0)
            SellMarket(Volume + Math.Abs(Position));
    }
}
```

In this example, risk management rules are set when the strategy starts. When a loss of 10000 is reached, positions will be automatically closed. When the position size exceeds 500 contracts, trading will be blocked. When orders are placed too frequently, active orders will be canceled. All these checks are performed automatically without any additional code in the `ProcessCandle` method.
