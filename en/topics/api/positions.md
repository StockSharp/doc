# Position Management

StockSharp provides a flexible position management system that allows you to track the current state of positions, calculate them based on orders or trades, and maintain a lifecycle history (opening, closing, reversals).

## PositionManager

The [PositionManager](xref:StockSharp.Algo.Positions.PositionManager) class implements the [IPositionManager](xref:StockSharp.Algo.Positions.IPositionManager) interface and serves as the primary component for calculating current positions based on incoming messages.

### Creating the Manager

The constructor accepts two parameters:

```cs
var state = new PositionManagerState();
var manager = new PositionManager(byOrders: false, state);
```

- `byOrders = true` -- the position is calculated based on order balance changes. Suitable when the trading system receives order state updates but not individual trades.
- `byOrders = false` -- the position is calculated based on trade volumes (recommended mode). Provides more accurate accounting of executed operations.

### Processing Messages

The `ProcessMessage` method accepts an incoming message ([Message](xref:StockSharp.Messages.Message)) and returns a [PositionChangeMessage](xref:StockSharp.Messages.PositionChangeMessage) when the position changes, or `null` if the position has not changed:

```cs
var posChange = manager.ProcessMessage(executionMsg);

if (posChange != null)
{
    Console.WriteLine($"Position: {posChange.CurrentValue}");
}
```

## IPositionManagerState

The [IPositionManagerState](xref:StockSharp.Algo.Positions.IPositionManagerState) interface describes the internal state of the position manager. The [PositionManagerState](xref:StockSharp.Algo.Positions.PositionManagerState) implementation stores information about current orders and positions.

### Main Methods

| Method | Description |
|--------|-------------|
| `AddOrGetOrder` | Registers a new order or returns an existing one by `transactionId` |
| `TryGetOrder` | Retrieves order parameters (instrument, portfolio, direction, balance) |
| `UpdateOrderBalance` | Updates the current order balance after partial execution |
| `RemoveOrder` | Removes a completed order from tracking |
| `UpdatePosition` | Updates the position by instrument and portfolio, returns the new value |
| `Clear` | Resets all manager state |

### Working with State Example

```cs
var state = new PositionManagerState();

// Register an order
state.AddOrGetOrder(
    transactionId: 12345,
    securityId: secId,
    portfolioName: "MyPortfolio",
    side: Sides.Buy,
    volume: 100,
    balance: 100
);

// Update after partial execution
state.UpdateOrderBalance(12345, newBalance: 60);

// Update position directly
var newPosition = state.UpdatePosition(secId, "MyPortfolio", diff: 40);
Console.WriteLine($"Current position: {newPosition}");

// Clear
state.Clear();
```

## PositionLifecycleTracker

The [PositionLifecycleTracker](xref:StockSharp.Algo.Positions.PositionLifecycleTracker) class tracks the complete lifecycle of positions -- from opening to closing (round-trip). This is useful for analyzing individual trades, calculating profit for each position, and generating reports.

### Key Features

- **History**: the `History` property (`IReadOnlyList<ReportPosition>`) contains all completed round-trip positions.
- **`RoundTripClosed` event**: fires when a position is closed (value reached zero) or reversed (position sign changed).
- **`ProcessPosition` method**: accepts a [Position](xref:StockSharp.BusinessEntities.Position) object and updates the internal state.

### Detected States

| State | Description |
|-------|-------------|
| Opening | Position transitions from zero to a non-zero value |
| Closing | Position value reaches zero |
| Reversal | Position sign changes (e.g., from long to short) |

### Usage Example

```cs
var tracker = new PositionLifecycleTracker();

tracker.RoundTripClosed += report =>
{
    Console.WriteLine($"Round-trip completed:");
    Console.WriteLine($"  Opened: {report.OpenTime}");
    Console.WriteLine($"  Closed: {report.CloseTime}");
};

// Process position updates
tracker.ProcessPosition(position);

// View history
foreach (var report in tracker.History)
{
    Console.WriteLine($"  {report.OpenTime} -> {report.CloseTime}");
}
```

## PositionMessageAdapter

The [PositionMessageAdapter](xref:StockSharp.Algo.Positions.PositionMessageAdapter) class is a wrapper around a message adapter that automatically calculates positions from the message stream. It is used within the internal connector infrastructure.

### How It Works

```cs
var innerAdapter = connector.Adapter;
var posManager = new PositionManager(byOrders: false, new PositionManagerState());
var posAdapter = new PositionMessageAdapter(innerAdapter, posManager);
```

The adapter intercepts order execution and trade messages, calls `PositionManager.ProcessMessage`, and generates corresponding `PositionChangeMessage` instances for upstream handlers.

## Positions in Strategies

In the [Strategy](xref:StockSharp.Algo.Strategies.Strategy) class, the current position is accessed through the `Position` property:

```cs
// Current position for the primary instrument
decimal currentPosition = Position;

// Close position
if (Position > 0)
    SellMarket(Math.Abs(Position));
else if (Position < 0)
    BuyMarket(Math.Abs(Position));

// Or through a built-in method
ClosePosition();
```

For more details about trading operations in strategies, see the [Trading Operations](strategies/trading_operations.md) section.

## See Also

- [Trading Operations](strategies/trading_operations.md)
- [Position Protection](strategies/take_profit_and_stop_loss.md)
- [Target Position Management](strategies/target_position_management.md)
- [Reporting](strategies/reporting.md)
