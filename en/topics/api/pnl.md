# Profit and Loss Management

[S\#](../api.md) implements profit and loss (PnL) calculation through the [PnLManager](xref:StockSharp.Algo.PnL.PnLManager). The manager processes a stream of messages (trades, market data) and computes realized and unrealized profit.

## IPnLManager Interface

The [IPnLManager](xref:StockSharp.Algo.PnL.IPnLManager) interface defines the base contract:

- **RealizedPnL** — realized profit/loss (decimal). Accumulated when positions are closed.
- **UnrealizedPnL** — unrealized profit/loss (decimal). Recalculated based on current market prices.
- **Reset()** — resets the state of the manager.
- **UpdateSecurity(Level1ChangeMessage)** — updates instrument parameters (price step, step price, lot multiplier).
- **ProcessMessage(Message, ICollection\<PortfolioPnLManager\>)** — processes a message; returns [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) when a position is closed, otherwise `null`.

## Architecture

The PnL system has a three-level hierarchy:

```
PnLManager
  └── PortfolioPnLManager (by portfolio name)
        └── PnLQueue (by SecurityId)
```

- [PnLManager](xref:StockSharp.Algo.PnL.PnLManager) — top level, manages a dictionary of portfolio managers.
- [PortfolioPnLManager](xref:StockSharp.Algo.PnL.PortfolioPnLManager) — PnL manager for a specific portfolio, manages queues by instrument.
- [PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) — FIFO queue for matching trades on a single instrument.

### PnLQueue — Calculation Queue

[PnLQueue](xref:StockSharp.Algo.PnL.PnLQueue) is responsible for matching opening and closing trades:

- **PriceStep** — instrument price step.
- **StepPrice** — price step cost (for futures).
- **Leverage** — leverage.
- **LotMultiplier** — lot multiplier.

The profit multiplier is calculated using the formula:

```
Multiplier = (StepPrice / PriceStep) * Leverage * LotMultiplier
```

For regular stocks (where `StepPrice` is not set), the multiplier equals `1 * Leverage * LotMultiplier`.

## PnLInfo — Trade Processing Result

The [PnLInfo](xref:StockSharp.Algo.PnL.PnLInfo) class contains the result of closing a position:

- **ServerTime** — trade time.
- **ClosedVolume** — volume of the closed position.
- **PnL** — realized profit from this trade.

For example, if the position was +2 and a trade for -5 contracts arrived, then `ClosedVolume = 2` (2 contracts from the position were closed).

## Configuring Data Sources

[PnLManager](xref:StockSharp.Algo.PnL.PnLManager) allows you to select market data sources for unrealized profit calculation:

| Property | Default | Description |
|----------|:-------:|-------------|
| `UseTick` | `true` | Use tick trades. |
| `UseOrderBook` | `false` | Use order book (best bid/ask). |
| `UseLevel1` | `false` | Use Level1 data. |
| `UseOrderLog` | `false` | Use order log. |
| `UseCandles` | `true` | Use candles (close price). |

## Integration via Adapter

The [PnLMessageAdapter](xref:StockSharp.Algo.PnL.PnLMessageAdapter) class wraps an inner adapter and automatically processes all messages for PnL calculation.

## Integration with Strategy

The strategy ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) provides:

- `PnLManager` property — the manager instance.
- `PnL` property — total profit (`RealizedPnL + UnrealizedPnL`).
- `PnLChanged` event — notification of profit changes.
- `PnLReceived2` event — notification when new PnL data is received.

## Usage Example

```cs
var pnlManager = new PnLManager
{
    UseTick = true,
    UseOrderBook = true,
    UseCandles = true
};

// Processing messages
var info = pnlManager.ProcessMessage(executionMsg);
if (info != null)
{
    Console.WriteLine($"Closed: {info.ClosedVolume}, PnL: {info.PnL}");
}

// Total profit/loss
var realizedPnL = pnlManager.RealizedPnL;
var unrealizedPnL = pnlManager.UnrealizedPnL;
var totalPnL = realizedPnL + unrealizedPnL;

Console.WriteLine($"Realized PnL: {realizedPnL}");
Console.WriteLine($"Unrealized PnL: {unrealizedPnL}");
Console.WriteLine($"Total PnL: {totalPnL}");
```

## Resetting State

The `Reset()` method clears all portfolio managers, calculation queues, and resets realized PnL to zero:

```cs
pnlManager.Reset();
```
