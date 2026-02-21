# Async Order Operations

The [Connector](xref:StockSharp.Algo.Connector) class provides asynchronous versions of all order operations. Async methods avoid blocking the calling thread and support cancellation via `CancellationToken`.

## Methods

### RegisterOrderAsync

Asynchronous registration of a new order:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

The method validates the order (checks the volume, automatically determines the order type -- limit or market), initializes the transaction, and sends the registration command to the adapter. In case of an error, a registration error event is generated.

The synchronous counterpart [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) internally calls `RegisterOrderAsync`.

### CancelOrderAsync

Asynchronous cancellation of an existing order:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

The method creates a new transaction identifier for the cancel operation and sends the order withdrawal command to the adapter. The order must have been previously registered.

Synchronous counterpart: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)).

### EditOrderAsync

Asynchronous editing of an active order (changing price and/or volume without cancellation):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

Parameters:
- `order` -- the original order to edit.
- `changes` -- an [Order](xref:StockSharp.BusinessEntities.Order) object with the new field values (price, volume, etc.).

Before calling, it is recommended to check editing support:

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

Synchronous counterpart: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)).

### ReRegisterOrderAsync

Asynchronous re-registration of an order (canceling the old one and registering a new one in a single operation):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

Used when the exchange does not support order editing but supports atomic replacement. Support can be checked via `IsOrderReplaceable`:

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## When to Use Async Methods

**Use async methods** when:
- Code executes in an `async` context (e.g., in ASP.NET handlers, cross-platform applications).
- You need to support operation cancellation via `CancellationToken`.
- You need to avoid blocking the UI thread.

**Use synchronous methods** when:
- Code runs within a strategy (`Strategy`), which internally manages threads.
- A simple script or console application where async is not required.

Synchronous methods (`RegisterOrder`, `CancelOrder`, `EditOrder`) internally call their async counterparts via `AsyncHelper.Run`, so they are fully equivalent in functionality.

## Example

```cs
private readonly Connector _connector = new();

public async Task PlaceAndManageOrderAsync(Security security, Portfolio portfolio, CancellationToken cancellationToken)
{
    // Create an order
    var order = new Order
    {
        Security = security,
        Portfolio = portfolio,
        Direction = Sides.Buy,
        Volume = 1,
        Price = security.BestBid?.Price ?? 100m,
        Type = OrderTypes.Limit,
    };

    // Async registration
    await _connector.RegisterOrderAsync(order, cancellationToken);

    // ... wait for market condition changes ...

    // Async price edit (if supported)
    if (_connector.IsOrderEditable(order) == true)
    {
        var changes = order.CreateOrder();
        changes.Price = order.Price - 0.01m;
        await _connector.EditOrderAsync(order, changes, cancellationToken);
    }

    // Async cancellation
    await _connector.CancelOrderAsync(order, cancellationToken);
}
```

## See Also

[Orders](../orders.md)
