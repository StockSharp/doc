# Asynchrone Order-Operationen

Die Klasse [Connector](xref:StockSharp.Algo.Connector) bietet asynchrone Versionen aller Order-Operationen. Asynchrone Methoden blockieren den aufrufenden Thread nicht und unterstützen die Abbruchmöglichkeit über `CancellationToken`.

## Methoden

### RegisterOrderAsync

Asynchrone Registrierung einer neuen Order:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

Die Methode validiert die Order (prüft das Volumen, ermittelt automatisch den Ordertyp -- Limit oder Market), initialisiert die Transaktion und sendet den Registrierungsbefehl an den Adapter. Im Fehlerfall wird ein Registrierungsfehler-Ereignis erzeugt.

Das synchrone Gegenstück [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) ruft intern `RegisterOrderAsync` auf.

### CancelOrderAsync

Asynchrone Stornierung einer bestehenden Order:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

Die Methode erstellt einen neuen Transaktionsbezeichner für den Stornierungsvorgang und sendet den Befehl zur Orderrücknahme an den Adapter. Die Order muss zuvor registriert worden sein.

Synchrones Gegenstück: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)).

### EditOrderAsync

Asynchrone Bearbeitung einer aktiven Order (Änderung von Preis und/oder Volumen ohne Stornierung):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

Parameter:
- `order` -- die ursprüngliche zu bearbeitende Order.
- `changes` -- ein [Order](xref:StockSharp.BusinessEntities.Order)-Objekt mit den neuen Feldwerten (Preis, Volumen usw.).

Vor dem Aufruf wird empfohlen, die Unterstützung der Bearbeitung zu prüfen:

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

Synchrones Gegenstück: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)).

### ReRegisterOrderAsync

Asynchrone Neuregistrierung einer Order (Stornierung der alten und Registrierung einer neuen in einem einzigen Vorgang):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

Wird verwendet, wenn die Börse keine Orderbearbeitung, aber einen atomaren Austausch unterstützt. Die Unterstützung kann über `IsOrderReplaceable` geprüft werden:

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## Wann asynchrone Methoden verwendet werden sollten

**Asynchrone Methoden verwenden**, wenn:
- der Code in einem `async`-Kontext ausgeführt wird (z. B. in ASP.NET-Handlern, plattformübergreifenden Anwendungen).
- Sie den Abbruch der Operation über `CancellationToken` unterstützen müssen.
- Sie eine Blockierung des UI-Threads vermeiden müssen.

**Synchrone Methoden verwenden**, wenn:
- der Code innerhalb einer Strategie (`Strategy`) ausgeführt wird, die intern die Threads verwaltet.
- ein einfaches Skript oder eine Konsolenanwendung vorliegt, bei der Async nicht erforderlich ist.

Synchrone Methoden (`RegisterOrder`, `CancelOrder`, `EditOrder`) rufen intern ihre asynchronen Gegenstücke über `AsyncHelper.Run` auf und sind daher funktional vollständig gleichwertig.

## Beispiel

```cs
private readonly Connector _connector = new();

public async Task PlaceAndManageOrderAsync(Security security, Portfolio portfolio, CancellationToken cancellationToken)
{
    // Order erstellen
    var order = new Order
    {
        Security = security,
        Portfolio = portfolio,
        Direction = Sides.Buy,
        Volume = 1,
        Price = security.BestBid?.Price ?? 100m,
        Type = OrderTypes.Limit,
    };

    // Asynchrone Registrierung
    await _connector.RegisterOrderAsync(order, cancellationToken);

    // ... auf Änderungen der Marktbedingungen warten ...

    // Async price edit (if supported)
    if (_connector.IsOrderEditable(order) == true)
    {
        var changes = order.CreateOrder();
        changes.Price = order.Price - 0.01m;
        await _connector.EditOrderAsync(order, changes, cancellationToken);
    }

    // Asynchrone Stornierung
    await _connector.CancelOrderAsync(order, cancellationToken);
}
```

## Siehe auch

[Orderverwaltung](../orders_management.md)
