# Operações Assíncronas de Ordens

A classe [Connector](xref:StockSharp.Algo.Connector) fornece versões assíncronas de todas as operações de ordens. Os métodos assíncronos evitam o bloqueio da thread de chamada e oferecem suporte ao cancelamento por meio de `CancellationToken`.

## Métodos

### RegisterOrderAsync

Registro assíncrono de uma nova ordem:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

O método valida a ordem (verifica o volume, determina automaticamente o tipo de ordem -- limitada ou a mercado), inicializa a transação e envia o comando de registro ao adaptador. Em caso de erro, um evento de erro de registro é gerado.

O equivalente síncrono [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) chama internamente `RegisterOrderAsync`.

### CancelOrderAsync

Cancelamento assíncrono de uma ordem existente:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

O método cria um novo identificador de transação para a operação de cancelamento e envia o comando de retirada da ordem ao adaptador. A ordem deve ter sido registrada previamente.

Equivalente síncrono: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)).

### EditOrderAsync

Edição assíncrona de uma ordem ativa (alteração de preço e/ou volume sem cancelamento):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

Parâmetros:
- `order` -- a ordem original a ser editada.
- `changes` -- um objeto [Order](xref:StockSharp.BusinessEntities.Order) com os novos valores dos campos (preço, volume, etc.).

Antes de chamar, recomenda-se verificar o suporte à edição:

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

Equivalente síncrono: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)).

### ReRegisterOrderAsync

Nova assinatura assíncrona de uma ordem (cancelando a antiga e registrando uma nova em uma única operação):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

Usado quando a bolsa não oferece suporte à edição de ordens, mas suporta a substituição atômica. O suporte pode ser verificado por meio de `IsOrderReplaceable`:

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## Quando Usar Métodos Assíncronos

**Use métodos assíncronos** quando:
- O código é executado em um contexto `async` (por exemplo, em manipuladores ASP.NET, aplicações multiplataforma).
- Você precisa oferecer suporte ao cancelamento da operação via `CancellationToken`.
- Você precisa evitar o bloqueio da thread de UI.

**Use métodos síncronos** quando:
- O código é executado dentro de uma estratégia (`Strategy`), que gerencia internamente as threads.
- Um script simples ou aplicação de console em que async não é necessário.

Os métodos síncronos (`RegisterOrder`, `CancelOrder`, `EditOrder`) chamam internamente seus equivalentes assíncronos via `AsyncHelper.Run`, portanto são totalmente equivalentes em funcionalidade.

## Exemplo

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

## Veja Também

[Ordens](../orders_management.md)
