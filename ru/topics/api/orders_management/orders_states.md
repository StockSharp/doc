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

## OrderStates enum

Заявка во время своей жизни проходит следующие состояния:

![OrderStates](../../../images/orderstates.png)

- [OrderStates.None](xref:StockSharp.Messages.OrderStates.None) - заявка была создана в роботе и еще не была отправлена на регистрацию. 
- [OrderStates.Pending](xref:StockSharp.Messages.OrderStates.Pending) - заявка была отправлена на регистрацию ([RegisterOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.RegisterOrder(StockSharp.BusinessEntities.Order)). Для заявки ожидается подтверждение ее принятия от биржи. В случае успеха принятия будет вызвано событие [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived), и заявка будет переведена в состояние [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active). Также будут проинициализированы свойства [Order.Id](xref:StockSharp.BusinessEntities.Order.Id) и [Order.ServerTime](xref:StockSharp.BusinessEntities.Order.ServerTime). В случае отвержения заявки будет вызвано событие [OrderRegisterFailReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderRegisterFailReceived) с описанием ошибки, и заявка будет переведена в состояние [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed). 
- [OrderStates.Active](xref:StockSharp.Messages.OrderStates.Active) - заявка активна на бирже. Такая заявка будет активна до тех пор, пока не исполнится весь ее выставленный объем [Order.Volume](xref:StockSharp.BusinessEntities.Order.Volume), или она не будет снята принудительно через [CancelOrder](xref:StockSharp.BusinessEntities.ITransactionProvider.CancelOrder(StockSharp.BusinessEntities.Order). Если заявка исполняется частично, то вызываются события [OwnTradeReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OwnTradeReceived) о новых сделках по выставленной заявке, а так же событие [OrderReceived](xref:StockSharp.BusinessEntities.ISubscriptionProvider.OrderReceived), где передается уведомление об изменении баланса по заявке [Order.Balance](xref:StockSharp.BusinessEntities.Order.Balance). Последнее событие будет выведено и в случае отмены заявки.
- [OrderStates.Done](xref:StockSharp.Messages.OrderStates.Done) - заявка более не активна на бирже (была полностью исполнена или снята). 
- [OrderStates.Failed](xref:StockSharp.Messages.OrderStates.Failed) - заявка не была принята биржей (или промежуточной системой, как, например, серверная часть торговой платформы) по какой\-либо причине. 

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

- [Подписки](../market_data/subscriptions.md)
- [Состояния заявок](orders_states.md)
- [Создание новой заявки](create_new_order.md)
- [Снятие заявок](order_cancel.md)