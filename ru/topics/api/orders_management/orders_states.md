# Состояния заявок

StockSharp API предоставляет возможность получать информацию о заявках посредством встроенного механизма подписок. Как и с рыночными данными, для транзакционной информации используется единый подход, основанный на [Subscription](xref:StockSharp.BusinessEntities.Subscription).

## События, связанные с заявками

[Connector](xref:StockSharp.Algo.Connector) предоставляет следующие события для обработки информации по заявкам:

| Событие | Описание |
|---------|----------|
| [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) | Событие получения информации о заявке |
| [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) | Событие ошибки регистрации заявки |
| [OrderCancelFailReceived](xref:StockSharp.Algo.Connector.OrderCancelFailReceived) | Событие ошибки отмены заявки |
| [OrderEditFailReceived](xref:StockSharp.Algo.Connector.OrderEditFailReceived) | Событие ошибки редактирования заявки |
| [OwnTradeReceived](xref:StockSharp.Algo.Connector.OwnTradeReceived) | Событие получения информации о собственной сделке |

## Автоматические подписки

По умолчанию [Connector](xref:StockSharp.Algo.Connector) автоматически создает подписки на транзакционную информацию при подключении ([SubscriptionsOnConnect](xref:StockSharp.Algo.Connector.SubscriptionsOnConnect)). Это включает подписки на:

- Информацию о заявках
- Информацию о сделках
- Информацию о позициях
- Базовый поиск инструментов

Пример обработки события получения заявки:

```cs
private void InitConnector()
{
    // Подписка на событие получения заявки
    Connector.OrderReceived += OnOrderReceived;
    
    // Подписка на событие получения собственной сделки
    Connector.OwnTradeReceived += OnOwnTradeReceived;
    
    // Подписка на событие ошибки регистрации заявки
    Connector.OrderRegisterFailReceived += OnOrderRegisterFailed;
}

private void OnOrderReceived(Subscription subscription, Order order)
{
    // Обработка полученной заявки
    _ordersWindow.OrderGrid.Orders.TryAdd(order);
    
    // Важно! Проверяем, относится ли заявка к текущей подписке
    // чтобы избежать дублирования обработки
    if (subscription == _myOrdersSubscription)
    {
        // Дополнительная обработка для конкретной подписки
        Console.WriteLine($"Заявка: {order.TransactionId}, Состояние: {order.State}");
    }
}
```

## Ручное создание подписок на заявки

В некоторых случаях может потребоваться явно запросить информацию о заявках. Для этого можно создать отдельные подписки:

```cs
// Создаем подписку на заявки по конкретному портфелю
var ordersSubscription = new Subscription(DataType.Transactions, portfolio)
{
    TransactionId = Connector.TransactionIdGenerator.GetNextId(),
};

// Обработчик получения заявок
Connector.OrderReceived += (subscription, order) =>
{
    if (subscription == ordersSubscription)
    {
        Console.WriteLine($"Заявка: {order.TransactionId}, Состояние: {order.State}, Портфель: {order.Portfolio.Name}");
    }
};

// Запускаем подписку
Connector.Subscribe(ordersSubscription);
```

## Проверка состояния заявок

Для определения текущего состояния заявки используются методы-расширения:

```cs
// Проверка состояния заявки
Order order = ...; // полученная заявка

// Отменена ли заявка
bool isCanceled = order.IsCanceled();

// Исполнена ли заявка полностью
bool isMatched = order.IsMatched();

// Исполнена ли заявка частично
bool isPartiallyMatched = order.IsMatchedPartially();

// Исполнена ли хотя бы часть заявки
bool isNotEmpty = order.IsMatchedEmpty();

// Получить исполненный объем
decimal matchedVolume = order.GetMatchedVolume();
```

## Расширенный подход: работа с несколькими подписками

В сложных сценариях может потребоваться работа с несколькими подписками на заявки одновременно. В этом случае важно правильно обрабатывать события, чтобы избежать дублирования:

```cs
private Subscription _portfolio1OrdersSubscription;
private Subscription _portfolio2OrdersSubscription;

private void RequestOrdersForDifferentPortfolios()
{
    // Подписка на заявки по первому портфелю
    _portfolio1OrdersSubscription = new Subscription(DataType.Transactions, _portfolio1);
    
    // Подписка на заявки по второму портфелю
    _portfolio2OrdersSubscription = new Subscription(DataType.Transactions, _portfolio2);
    
    // Общий обработчик получения заявок
    Connector.OrderReceived += OnMultipleSubscriptionOrderReceived;
    
    // Запускаем подписки
    Connector.Subscribe(_portfolio1OrdersSubscription);
    Connector.Subscribe(_portfolio2OrdersSubscription);
}

private void OnMultipleSubscriptionOrderReceived(Subscription subscription, Order order)
{
    // Определяем, к какой подписке относится заявка
    if (subscription == _portfolio1OrdersSubscription)
    {
        // Обработка заявок первого портфеля
    }
    else if (subscription == _portfolio2OrdersSubscription)
    {
        // Обработка заявок второго портфеля
    }
}
```

> [!NOTE]
> Такой продвинутый подход с несколькими подписками на заявки следует использовать только в исключительных случаях, когда стандартного механизма подписок недостаточно.

## Асинхронная природа транзакций

Отправка транзакций (регистрация, замена или отмена заявок) выполняется в асинхронном режиме. Это позволяет торговой программе не ожидать подтверждения от биржи, а продолжать работу, что ускоряет реакцию на изменения рыночной ситуации.

Для отслеживания статуса заявки необходимо подписаться на соответствующие события:
- [OrderReceived](xref:StockSharp.Algo.Connector.OrderReceived) для получения обновлений состояния заявки
- [OrderRegisterFailReceived](xref:StockSharp.Algo.Connector.OrderRegisterFailReceived) для обработки ошибок регистрации

## См. также

- [Подписки](subscriptions.md)
- [Состояния заявок](orders_states.md)
- [Создание новой заявки](create_new_order.md)
- [Снятие заявок](order_cancel.md)