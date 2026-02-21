# Subscription Lifecycle

Subscriptions in StockSharp go through specific lifecycle stages. The [ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) interface provides events for tracking each stage.

## Lifecycle Events

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

Called when a subscription has been successfully started -- the adapter has accepted the request and begun transmitting data. For historical subscriptions, this means data loading has begun. For live subscriptions -- that the server has accepted the request.

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

Called when the subscription has transitioned to real-time mode. For live subscriptions, this means that the historical catch-up (if any) has completed and data is now arriving in real time. This is an important signal for strategies to understand that indicators are "warmed up" and trading can begin.

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

Called when the subscription has ended. The `Exception` parameter contains the reason for stopping:
- `null` -- normal completion (user unsubscribed or historical data finished).
- An exception object -- an error (connection loss, server-side error, etc.).

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

Called on a subscription error. The third `bool` parameter indicates whether this was a subscribe (`true`) or unsubscribe (`false`) operation.

## Event Order

Typical sequence for a live subscription:

1. Call `Subscribe(subscription)`
2. `SubscriptionStarted` -- subscription accepted
3. Data arriving (candles, order books, trades, etc.)
4. `SubscriptionOnline` -- transition to real-time mode
5. Continued data arrival in real time
6. Call `UnSubscribe(subscription)` or connection loss
7. `SubscriptionStopped` -- subscription ended

For a historical subscription (with a specified date range):

1. Call `Subscribe(subscription)`
2. `SubscriptionStarted` -- subscription accepted
3. Historical data arriving
4. `SubscriptionStopped` with `null` -- all data received

## SubscriptionsOnConnect

The [Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) property defines the set of subscriptions that are automatically sent on connection:

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

By default, subscriptions for security lookups, portfolio lookups, and order lookups are included:

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

You can add your own subscriptions that will be automatically started on each connection:

```cs
// Add automatic Level1 data subscription
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// Remove automatic order lookup on connection
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## Per-Adapter Connection Events

When working with multiple connections (multiple adapters), events indicating which specific adapter connected or disconnected are useful:

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

Called on successful connection of a specific adapter. The parameter is the adapter that initiated the event.

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

Called on disconnection of a specific adapter.

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

Called on a connection error for a specific adapter.

Aggregated events `Connected`, `Disconnected`, and `ConnectionError` are also available, which fire without specifying a particular adapter.

## Example

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // Track subscription lifecycle
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Subscription started: {subscription.DataType}, " +
            $"Security: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Subscription online: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Subscription completed: {subscription.DataType}");
        else
            Console.WriteLine($"Subscription interrupted: {subscription.DataType}, " +
                $"Error: {error.Message}");
    };

    // Track individual adapter connections
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter connected: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Adapter disconnected: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Adapter connection error {adapter.Name}: {error.Message}");
    };

    // Connect
    _connector.Connect();

    // After connection -- create a subscription
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## See Also

[Connection](../connectors.md)
