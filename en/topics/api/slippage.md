# Slippage Measurement

[S\#](../api.md) calculates slippage through the [SlippageManager](xref:StockSharp.Algo.Slippage.SlippageManager). Slippage is the difference between the expected execution price of an order and the actual trade price.

## ISlippageManager Interface

The [ISlippageManager](xref:StockSharp.Algo.Slippage.ISlippageManager) interface defines the base contract:

- **Slippage** — total accumulated slippage (decimal).
- **Reset()** — resets the state of the manager.
- **ProcessMessage(Message)** — processes a message; returns the slippage for the given execution or `null`.

## How It Works

The slippage manager works in three stages:

### 1. Updating Market Prices

When a [Level1ChangeMessage](xref:StockSharp.Messages.Level1ChangeMessage) or [QuoteChangeMessage](xref:StockSharp.Messages.QuoteChangeMessage) is received, the manager saves the best bid and ask prices for each instrument.

### 2. Saving the Planned Price

When an [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) is received, the manager saves the "planned price" — the best market price at the time of order registration:

- For a buy (`Buy`), the best ask is used.
- For a sell (`Sell`), the best bid is used.

### 3. Calculating Slippage

When an [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) with a trade is received, the manager calculates slippage:

- For a buy: `(TradePrice - PlannedPrice) * TradeVolume`
- For a sell: `(PlannedPrice - TradePrice) * TradeVolume`

A positive value means unfavorable slippage (price worse than expected), while a negative value means favorable slippage (price better than expected).

## State: ISlippageManagerState

The [ISlippageManagerState](xref:StockSharp.Algo.Slippage.ISlippageManagerState) interface stores the manager's internal state:

- Best bid/ask prices for each instrument ([SecurityId](xref:StockSharp.Messages.SecurityId)).
- Planned prices and directions for each transaction (`TransactionId`).
- Total accumulated slippage.

The default implementation is [SlippageManagerState](xref:StockSharp.Algo.Slippage.SlippageManagerState).

## Settings

| Property | Default | Description |
|----------|:-------:|-------------|
| `CalculateNegative` | `true` | Account for favorable slippage. If `false`, negative values are replaced with zero. |

## Integration via Adapter

The [SlippageMessageAdapter](xref:StockSharp.Algo.Slippage.SlippageMessageAdapter) class wraps an inner adapter and automatically calculates slippage for all trades.

## Integration with Strategy

The strategy ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) exposes the `Slippage` property for tracking overall slippage.

## Usage Example

```cs
// Creating a manager with a state store
var manager = new SlippageManager(new SlippageManagerState());

// Only account for unfavorable slippage
manager.CalculateNegative = false;

// Processing market data (updating best prices)
manager.ProcessMessage(level1Msg);
manager.ProcessMessage(quoteChangeMsg);

// Processing order registration (saving the planned price)
manager.ProcessMessage(orderRegisterMsg);

// Processing a trade (calculating slippage)
decimal? slippage = manager.ProcessMessage(executionMsg);
if (slippage != null)
{
    Console.WriteLine($"Slippage: {slippage.Value}");
}

// Total accumulated slippage
Console.WriteLine($"Total slippage: {manager.Slippage}");
```

## Resetting State

The `Reset()` method completely clears the internal state: best prices, planned prices, and accumulated slippage:

```cs
manager.Reset();
```
