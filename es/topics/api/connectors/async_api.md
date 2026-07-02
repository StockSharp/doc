# Operaciones asíncronas con órdenes

La clase [Connector](xref:StockSharp.Algo.Connector) proporciona versiones asíncronas de todas las operaciones con órdenes. Los métodos asíncronos evitan bloquear el hilo que realiza la llamada y admiten la cancelación mediante `CancellationToken`.

## Métodos

### RegisterOrderAsync

Registro asíncrono de una nueva orden:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

El método valida la orden (comprueba el volumen, determina automáticamente el tipo de orden -- límite o mercado), inicializa la transacción y envía el comando de registro al adaptador. En caso de error, se genera un evento de error de registro.

La contraparte síncrona [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) llama internamente a `RegisterOrderAsync`.

### CancelOrderAsync

Cancelación asíncrona de una orden existente:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

El método crea un nuevo identificador de transacción para la operación de cancelación y envía el comando de retirada de la orden al adaptador. La orden debe haber sido registrada previamente.

Contraparte síncrona: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)).

### EditOrderAsync

Edición asíncrona de una orden activa (cambio de precio y/o volumen sin cancelación):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

Parámetros:
- `order` -- la orden original a editar.
- `changes` -- un objeto [Order](xref:StockSharp.BusinessEntities.Order) con los nuevos valores de los campos (precio, volumen, etc.).

Antes de llamar, se recomienda comprobar el soporte de edición:

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

Contraparte síncrona: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)).

### ReRegisterOrderAsync

Re-registro asíncrono de una orden (cancelación de la antigua y registro de una nueva en una sola operación):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

Se utiliza cuando el exchange no admite la edición de órdenes, pero sí admite el reemplazo atómico. El soporte se puede comprobar mediante `IsOrderReplaceable`:

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## Cuándo usar métodos asíncronos

**Use métodos asíncronos** cuando:
- El código se ejecuta en un contexto `async` (por ejemplo, en manejadores de ASP.NET, aplicaciones multiplataforma).
- Necesita admitir la cancelación de la operación mediante `CancellationToken`.
- Necesita evitar bloquear el hilo de la interfaz de usuario.

**Use métodos síncronos** cuando:
- El código se ejecuta dentro de una estrategia (`Strategy`), que administra internamente los hilos.
- Es un script simple o una aplicación de consola donde no se requiere async.

Los métodos síncronos (`RegisterOrder`, `CancelOrder`, `EditOrder`) llaman internamente a sus contrapartes asíncronas mediante `AsyncHelper.Run`, por lo que son completamente equivalentes en funcionalidad.

## Ejemplo

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

## Véase también

[Órdenes](../orders_management.md)
