# Жизненный цикл подписок

Подписки в StockSharp проходят через определенные этапы жизненного цикла. Интерфейс [ISubscriptionProvider](xref:StockSharp.BusinessEntities.ISubscriptionProvider) предоставляет события для отслеживания каждого этапа.

## События жизненного цикла

### SubscriptionStarted

```cs
event Action<Subscription> SubscriptionStarted;
```

Вызывается, когда подписка успешно запущена -- адаптер принял запрос и начал передачу данных. Для исторических подписок это означает начало загрузки данных. Для live-подписок -- что сервер принял запрос.

### SubscriptionOnline

```cs
event Action<Subscription> SubscriptionOnline;
```

Вызывается, когда подписка перешла в режим реального времени. Для live-подписок означает, что исторический "догон" (если был) завершен и теперь данные приходят в реальном времени. Это важный сигнал для стратегий, чтобы понять, что индикаторы "прогреты" и можно начинать торговлю.

### SubscriptionStopped

```cs
event Action<Subscription, Exception> SubscriptionStopped;
```

Вызывается, когда подписка завершена. Параметр `Exception` содержит причину остановки:
- `null` -- нормальное завершение (отписка пользователем или завершение исторических данных).
- Объект исключения -- ошибка (потеря связи, ошибка на стороне сервера и т.д.).

### SubscriptionFailed

```cs
event Action<Subscription, Exception, bool> SubscriptionFailed;
```

Вызывается при ошибке подписки. Третий параметр `bool` указывает, была ли это подписка (`true`) или отписка (`false`).

## Порядок событий

Типичная последовательность для live-подписки:

1. Вызов `Subscribe(subscription)`
2. `SubscriptionStarted` -- подписка принята
3. Поступление данных (свечи, стаканы, сделки и т.д.)
4. `SubscriptionOnline` -- переход в режим реального времени
5. Продолжение поступления данных в реальном времени
6. Вызов `UnSubscribe(subscription)` или потеря связи
7. `SubscriptionStopped` -- подписка завершена

Для исторической подписки (с заданным диапазоном дат):

1. Вызов `Subscribe(subscription)`
2. `SubscriptionStarted` -- подписка принята
3. Поступление исторических данных
4. `SubscriptionStopped` с `null` -- все данные получены

## SubscriptionsOnConnect

Свойство [Connector.SubscriptionsOnConnect](xref:StockSharp.Algo.Connector) определяет набор подписок, которые автоматически отправляются при подключении:

```cs
ISet<Subscription> SubscriptionsOnConnect { get; }
```

По умолчанию включены подписки на справочники инструментов, портфелей и заявок:

```cs
SubscriptionsOnConnect.Add(SecurityLookup);
SubscriptionsOnConnect.Add(PortfolioLookup);
SubscriptionsOnConnect.Add(OrderLookup);
```

Можно добавлять свои подписки, которые будут автоматически запускаться при каждом подключении:

```cs
// Добавить автоматическую подписку на Level1 данные
var l1Sub = new Subscription(DataType.Level1, security);
connector.SubscriptionsOnConnect.Add(l1Sub);

// Убрать автоматический запрос заявок при подключении
connector.SubscriptionsOnConnect.Remove(connector.OrderLookup);
```

## Пер-адаптерные события подключения

При работе с несколькими подключениями (множественные адаптеры) полезны события, указывающие, какой именно адаптер подключился или отключился:

### ConnectedEx

```cs
event Action<IMessageAdapter> ConnectedEx;
```

Вызывается при успешном подключении конкретного адаптера. Параметр -- адаптер, инициировавший событие.

### DisconnectedEx

```cs
event Action<IMessageAdapter> DisconnectedEx;
```

Вызывается при отключении конкретного адаптера.

### ConnectionErrorEx

```cs
event Action<IMessageAdapter, Exception> ConnectionErrorEx;
```

Вызывается при ошибке подключения конкретного адаптера.

Также доступны общие (агрегированные) события `Connected`, `Disconnected` и `ConnectionError`, которые срабатывают без указания конкретного адаптера.

## Пример

```cs
private readonly Connector _connector = new();

public void SetupSubscriptionTracking()
{
    // Отслеживание жизненного цикла подписок
    _connector.SubscriptionStarted += subscription =>
    {
        Console.WriteLine($"Подписка запущена: {subscription.DataType}, " +
            $"Инструмент: {subscription.SecurityId}");
    };

    _connector.SubscriptionOnline += subscription =>
    {
        Console.WriteLine($"Подписка онлайн: {subscription.DataType}");
    };

    _connector.SubscriptionStopped += (subscription, error) =>
    {
        if (error == null)
            Console.WriteLine($"Подписка завершена: {subscription.DataType}");
        else
            Console.WriteLine($"Подписка прервана: {subscription.DataType}, " +
                $"Ошибка: {error.Message}");
    };

    // Отслеживание подключений отдельных адаптеров
    _connector.ConnectedEx += adapter =>
    {
        Console.WriteLine($"Адаптер подключен: {adapter.Name}");
    };

    _connector.DisconnectedEx += adapter =>
    {
        Console.WriteLine($"Адаптер отключен: {adapter.Name}");
    };

    _connector.ConnectionErrorEx += (adapter, error) =>
    {
        Console.WriteLine($"Ошибка подключения адаптера {adapter.Name}: {error.Message}");
    };

    // Подключение
    _connector.Connect();

    // После подключения -- создание подписки
    _connector.Connected += () =>
    {
        var subscription = new Subscription(DataType.Ticks, security);
        _connector.Subscribe(subscription);
    };
}
```

## См. также

[Подключение](../connectors.md)
