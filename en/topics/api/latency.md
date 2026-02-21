# Latency Measurement

[S\#](../api.md) measures order registration and cancellation latency through the [LatencyManager](xref:StockSharp.Algo.Latency.LatencyManager). The manager determines how much time passes between sending an order and receiving confirmation from the exchange.

## ILatencyManager Interface

The [ILatencyManager](xref:StockSharp.Algo.Latency.ILatencyManager) interface defines the base contract:

- **LatencyRegistration** — total registration latency across all orders (TimeSpan).
- **LatencyCancellation** — total cancellation latency across all orders (TimeSpan).
- **Reset()** — resets the state of the manager.
- **ProcessMessage(Message)** — processes a message; returns the latency for the given operation or `null`.

## How It Works

The latency manager operates on a "request-response" principle:

### 1. Order Registration

When an [OrderRegisterMessage](xref:StockSharp.Messages.OrderRegisterMessage) is received, the manager saves the pair (`TransactionId`, `LocalTime`) — the moment the order was sent.

### 2. Order Cancellation

When an [OrderCancelMessage](xref:StockSharp.Messages.OrderCancelMessage) is received, the manager saves the pair (`TransactionId`, `LocalTime`) — the moment the cancellation was sent.

### 3. Order Replacement

When an [OrderReplaceMessage](xref:StockSharp.Messages.OrderReplaceMessage) is received, the manager registers both a cancellation (of the old order) and a registration (of the new order) simultaneously.

### 4. Confirmation

When an [ExecutionMessage](xref:StockSharp.Messages.ExecutionMessage) with order information (not in `Pending` state and not `Failed`) is received, the manager calculates the latency:

```
Latency = ExecutionMessage.LocalTime - StoredLocalTime
```

The result is added to `LatencyRegistration` or `LatencyCancellation` depending on the operation type.

## State: ILatencyManagerState

The [ILatencyManagerState](xref:StockSharp.Algo.Latency.ILatencyManagerState) interface stores the manager's internal state:

- Pending registrations: `AddRegistration(transactionId, localTime)` / `TryGetAndRemoveRegistration(transactionId, out localTime)`
- Pending cancellations: `AddCancellation(transactionId, localTime)` / `TryGetAndRemoveCancellation(transactionId, out localTime)`
- Accumulated latencies: `LatencyRegistration`, `LatencyCancellation`
- Addition methods: `AddLatencyRegistration(TimeSpan)`, `AddLatencyCancellation(TimeSpan)`

The default implementation is [LatencyManagerState](xref:StockSharp.Algo.Latency.LatencyManagerState).

## Error Handling

If an order fails (`OrderState == Failed`), the latency is not counted — the record is simply removed from the state store.

## Integration via Adapter

The [LatencyMessageAdapter](xref:StockSharp.Algo.Latency.LatencyMessageAdapter) class wraps an inner adapter and automatically measures latency for all order operations.

## Integration with Strategy

The strategy ([Strategy](xref:StockSharp.Algo.Strategies.Strategy)) exposes the `Latency` property for tracking latency.

## Usage Example

```cs
// Creating a manager with a state store
var manager = new LatencyManager(new LatencyManagerState());

// Processing order registration (saving the send time)
manager.ProcessMessage(orderRegisterMsg);

// Processing confirmation (calculating latency)
TimeSpan? latency = manager.ProcessMessage(executionMsg);
if (latency != null)
{
    Console.WriteLine($"Latency: {latency.Value.TotalMilliseconds} ms");
}

// Total latencies
Console.WriteLine($"Registration latency: {manager.LatencyRegistration.TotalMilliseconds} ms");
Console.WriteLine($"Cancellation latency: {manager.LatencyCancellation.TotalMilliseconds} ms");
```

## Resetting State

The `Reset()` method clears all pending records and resets accumulated latencies to zero:

```cs
manager.Reset();
```
