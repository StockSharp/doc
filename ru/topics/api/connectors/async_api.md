# Асинхронные операции с заявками

Класс [Connector](xref:StockSharp.Algo.Connector) предоставляет асинхронные версии всех операций с заявками. Асинхронные методы позволяют не блокировать вызывающий поток и поддерживают механизм отмены через `CancellationToken`.

## Методы

### RegisterOrderAsync

Асинхронная регистрация новой заявки:

```cs
public async ValueTask RegisterOrderAsync(Order order, CancellationToken cancellationToken = default)
```

Метод выполняет валидацию заявки (проверяет объем, автоматически определяет тип заявки -- лимитная или рыночная), инициализирует транзакцию и отправляет команду регистрации в адаптер. В случае ошибки генерируется событие об ошибке регистрации.

Синхронный аналог [RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)) внутренне вызывает `RegisterOrderAsync`.

### CancelOrderAsync

Асинхронная отмена существующей заявки:

```cs
public async ValueTask CancelOrderAsync(Order order, CancellationToken cancellationToken = default)
```

Метод создает новый идентификатор транзакции для операции отмены и отправляет команду снятия заявки в адаптер. Заявка должна быть ранее зарегистрирована.

Синхронный аналог: [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order)).

### EditOrderAsync

Асинхронное редактирование активной заявки (изменение цены и/или объема без снятия):

```cs
public async ValueTask EditOrderAsync(Order order, Order changes, CancellationToken cancellationToken = default)
```

Параметры:
- `order` -- исходная заявка для редактирования.
- `changes` -- объект [Order](xref:StockSharp.BusinessEntities.Order) с новыми значениями полей (цена, объем и т.д.).

Перед вызовом рекомендуется проверить поддержку редактирования:

```cs
if (connector.IsOrderEditable(order) == true)
{
    var changes = order.CreateOrder();
    changes.Price = newPrice;
    await connector.EditOrderAsync(order, changes);
}
```

Синхронный аналог: [EditOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.EditOrder(StockSharp.BusinessEntities.Order,StockSharp.BusinessEntities.Order)).

### ReRegisterOrderAsync

Асинхронная перерегистрация заявки (снятие старой и регистрация новой одной операцией):

```cs
public async ValueTask ReRegisterOrderAsync(Order oldOrder, Order newOrder, CancellationToken cancellationToken = default)
```

Используется, когда биржа не поддерживает редактирование заявок, но поддерживает атомарную замену. Проверить поддержку можно через `IsOrderReplaceable`:

```cs
if (connector.IsOrderReplaceable(order) == true)
{
    var newOrder = order.CreateOrder();
    newOrder.Price = newPrice;
    await connector.ReRegisterOrderAsync(order, newOrder);
}
```

## Когда использовать асинхронные методы

**Используйте асинхронные методы**, когда:
- Код выполняется в `async`-контексте (например, в обработчиках ASP.NET, кроссплатформенных приложениях).
- Необходимо поддерживать отмену операций через `CancellationToken`.
- Нужно избежать блокировки UI-потока.

**Используйте синхронные методы**, когда:
- Код работает в стратегии (`Strategy`), которая внутренне управляет потоками.
- Простой скрипт или консольное приложение, где асинхронность не требуется.

Синхронные методы (`RegisterOrder`, `CancelOrder`, `EditOrder`) внутренне вызывают свои асинхронные аналоги через `AsyncHelper.Run`, поэтому они полностью эквивалентны по функциональности.

## Пример

```cs
private readonly Connector _connector = new();

public async Task PlaceAndManageOrderAsync(Security security, Portfolio portfolio, CancellationToken cancellationToken)
{
    // Создание заявки
    var order = new Order
    {
        Security = security,
        Portfolio = portfolio,
        Direction = Sides.Buy,
        Volume = 1,
        Price = security.BestBid?.Price ?? 100m,
        Type = OrderTypes.Limit,
    };

    // Асинхронная регистрация
    await _connector.RegisterOrderAsync(order, cancellationToken);

    // ... ожидание изменения рыночных условий ...

    // Асинхронное редактирование цены (если поддерживается)
    if (_connector.IsOrderEditable(order) == true)
    {
        var changes = order.CreateOrder();
        changes.Price = order.Price - 0.01m;
        await _connector.EditOrderAsync(order, changes, cancellationToken);
    }

    // Асинхронная отмена
    await _connector.CancelOrderAsync(order, cancellationToken);
}
```

## См. также

[Заявки](../orders.md)
