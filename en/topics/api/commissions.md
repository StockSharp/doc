# Commission System

[S\#](../api.md) implements a flexible commission calculation system through the [CommissionManager](xref:StockSharp.Algo.Commissions.CommissionManager). The manager accepts order and trade messages and calculates commissions based on configured rules.

## ICommissionManager Interface

The [ICommissionManager](xref:StockSharp.Algo.Commissions.ICommissionManager) interface defines the base contract:

- **Rules** — a collection of [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule) rules for commission calculation.
- **Commission** — the total accumulated commission amount (decimal).
- **Reset()** — resets the state of the manager and all rules.
- **Process(Message)** — processes a message; returns the commission for the given message or `null`.

## ICommissionRule Interface

Each rule implements [ICommissionRule](xref:StockSharp.Algo.Commissions.ICommissionRule):

- **Title** — the rule title.
- **Value** — the commission value ([Unit](xref:Ecng.ComponentModel.Unit)), can be absolute or percentage-based.
- **Process(ExecutionMessage)** — calculates the commission for a specific message.

The base class [CommissionRule](xref:StockSharp.Algo.Commissions.CommissionRule) contains a helper method `GetValue(price, volume)`:

- For **absolute** values, it returns `Value` as-is.
- For **percentage** values, it computes `(price * volume * Value) / 100`.

## Rule Types

### Order Rules

| Class | Description |
|-------|-------------|
| [CommissionOrderRule](xref:StockSharp.Algo.Commissions.CommissionOrderRule) | Commission per order (based on order price and volume). |
| [CommissionOrderVolumeRule](xref:StockSharp.Algo.Commissions.CommissionOrderVolumeRule) | Commission based on order volume. For absolute values: `Value * volume`. |
| [CommissionOrderCountRule](xref:StockSharp.Algo.Commissions.CommissionOrderCountRule) | Commission for every N orders. The `Count` property sets the threshold. |

### Trade Rules

| Class | Description |
|-------|-------------|
| [CommissionTradeRule](xref:StockSharp.Algo.Commissions.CommissionTradeRule) | Commission per trade (based on trade price and volume). |
| [CommissionTradeVolumeRule](xref:StockSharp.Algo.Commissions.CommissionTradeVolumeRule) | Commission based on trade volume. |
| [CommissionTradePriceRule](xref:StockSharp.Algo.Commissions.CommissionTradePriceRule) | Commission: `price * volume * Value`. |
| [CommissionTradeCountRule](xref:StockSharp.Algo.Commissions.CommissionTradeCountRule) | Commission for every N trades. The `Count` property sets the threshold. |
| [CommissionTurnOverRule](xref:StockSharp.Algo.Commissions.CommissionTurnOverRule) | Commission for each turnover threshold. The `TurnOver` property sets the threshold. |

### Filter Rules

| Class | Description |
|-------|-------------|
| [CommissionSecurityIdRule](xref:StockSharp.Algo.Commissions.CommissionSecurityIdRule) | Commission only for a specific instrument. `Security` property. |
| [CommissionBoardCodeRule](xref:StockSharp.Algo.Commissions.CommissionBoardCodeRule) | Commission only for a specific exchange board. `Board` property. |
| [CommissionSecurityTypeRule](xref:StockSharp.Algo.Commissions.CommissionSecurityTypeRule) | Commission only for a specific instrument type. `SecurityType` property. |

## Integration via Adapter

The [CommissionMessageAdapter](xref:StockSharp.Algo.Commissions.CommissionMessageAdapter) class wraps an inner adapter and automatically calculates commissions on incoming and outgoing [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage). If a message does not have the `Commission` field set, the adapter populates it from the manager.

## Integration with Strategy

The strategy ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) exposes the `Commission` property, through which you can track the accumulated commission.

## Usage Example

```cs
var manager = new CommissionManager();

// Fixed commission of 1.5 per trade
manager.Rules.Add(new CommissionTradeRule { Value = 1.5m });

// 0.1% of turnover for futures
manager.Rules.Add(new CommissionSecurityTypeRule
{
    SecurityType = SecurityTypes.Future,
    Value = new Unit(0.1m, UnitTypes.Percent)
});

// Commission of 50 for every 100 orders
manager.Rules.Add(new CommissionOrderCountRule
{
    Count = 100,
    Value = 50m
});

// Commission of 10 for every 1,000,000 in turnover
manager.Rules.Add(new CommissionTurnOverRule
{
    TurnOver = 1_000_000m,
    Value = 10m
});

// Processing a message
decimal? commission = manager.Process(executionMsg);
if (commission != null)
{
    Console.WriteLine($"Commission for message: {commission.Value}");
}

// Total accumulated commission
Console.WriteLine($"Total commission: {manager.Commission}");
```

## Resetting State

The `Reset()` method resets the total commission to zero and calls `Reset()` on each rule, which clears internal counters (order count, current turnover, etc.):

```cs
manager.Reset();
```
