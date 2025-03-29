# Заявки

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) - таблица для отображения заявок и условных заявок. Кроме того контекстное меню этой таблицы содержит команды для операций с заявками: регистрация, замена и отмена заявок. Выбор пункта меню приводит к генерации событий: [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering), [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) или [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) соответственно.

![GUI OrderGrid](../../../../images/gui_ordergrid.png)

> [!TIP]
> Сама операция (регистрация, замена, отмена) не выполняется. Соответствующий код нужно прописывать в обработчиках событий самостоятельно.

**Основные члены**

- [OrderGrid.Orders](xref:StockSharp.Xaml.OrderGrid.Orders) - список заявок.
- [OrderGrid.SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) - выбранная заявка.
- [OrderGrid.SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) - выбранные заявки.
- [OrderGrid.AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail))**(**[StockSharp.BusinessEntities.OrderFail](xref:StockSharp.BusinessEntities.OrderFail) fail **)** - метод, который добавляет сообщение об ошибке регистрации заявки в поле комментария.
- [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) - событие регистрации заявки (возникает после выбора соответствующего пункта контекстного меню).
- [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) - событие замены заявки (возникает после выбора соответствующего пункта контекстного меню).
- [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) - событие отмены заявки (возникает после выбора соответствующего пункта контекстного меню).

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```xaml
<Window x:Class="Sample.OrdersWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="{x:Static loc:LocalizedStrings.Orders}" Height="410" Width="930">
    <xaml:OrderGrid x:Name="OrderGrid" x:FieldModifier="public" 
                    OrderCanceling="OrderGrid_OnOrderCanceling" 
                    OrderReRegistering="OrderGrid_OnOrderReRegistering" />
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();

private void ConnectClick(object sender, RoutedEventArgs e)
{
    // Другой код при подключении...
    
    // Подписываемся на событие получения заявок
    _connector.OrderReceived += (subscription, order) => 
    {
        // Добавляем заявки в таблицу OrderGrid
        _ordersWindow.OrderGrid.Orders.TryAdd(order);
    };
    
    // Для подключения коннектора
    _connector.Connect();
}
              	
// Удаляет все выбранные заявки
private void OrderGrid_OnOrderCanceling(IEnumerable<Order> orders)
{
    // Перебираем выбранные заявки и отменяем каждую
    foreach (var order in orders)
    {
        _connector.CancelOrder(order);
    }
}

// Открывает окно редактирования заявки и выполняет замену выбранной заявки
private void OrderGrid_OnOrderReRegistering(Order order)
{
    var window = new OrderWindow
    {
        Title = LocalizedStrings.Str2976Params.Put(order.TransactionId),
        SecurityProvider = _connector,
        MarketDataProvider = _connector,
        Portfolios = new PortfolioDataSource(_connector),
        Order = order.ReRegisterClone(newVolume: order.Balance)
    };
    
    if (window.ShowModal(this))
        _connector.ReRegisterOrder(order, window.Order);
}
	  				
```

## Работа с заявками через подписки

Современный подход к работе с заявками предполагает использование подписок:

```cs
// Подписываемся на событие получения заявок
_connector.OrderReceived += OnOrderReceived;

// Обработчик получения заявки
private void OnOrderReceived(Subscription subscription, Order order)
{
    // Проверяем, относится ли заявка к интересующей нас подписке
    if (subscription == _ordersSubscription)
    {
        // Добавляем заявку в таблицу
        _ordersWindow.OrderGrid.Orders.TryAdd(order);
        
        // Дополнительная обработка заявки
        Console.WriteLine($"Получена заявка: {order.TransactionId}, Статус: {order.State}");
        
        // Если заявка в финальном состоянии, обновляем UI
        if (order.State == OrderStates.Done || order.State == OrderStates.Failed)
        {
            this.GuiAsync(() => {
                // Обновление интерфейса для завершенных заявок
            });
        }
    }
}
```

## Отмена заявок

```cs
// Современный подход к отмене заявок
private void CancelOrder(Order order)
{
    try
    {
        _connector.CancelOrder(order);
        
        // Логирование действия
        _logManager.AddInfoLog($"Отправлена команда на отмену заявки {order.TransactionId}");
    }
    catch (Exception ex)
    {
        _logManager.AddErrorLog($"Ошибка при отмене заявки: {ex.Message}");
    }
}

// Массовая отмена заявок
private void CancelAllOrders()
{
    var activeOrders = _ordersWindow.OrderGrid.Orders
        .Where(o => o.State == OrderStates.Active)
        .ToArray();
        
    foreach (var order in activeOrders)
    {
        CancelOrder(order);
    }
}
```

## Обработка ошибок регистрации и отмены заявок

```cs
// Подписка на ошибки регистрации заявок
_connector.OrderRegisterFailReceived += OnOrderRegisterFailed;

// Обработчик ошибки регистрации заявки
private void OnOrderRegisterFailed(Subscription subscription, OrderFail fail)
{
    // Добавляем информацию об ошибке в OrderGrid
    _ordersWindow.OrderGrid.AddRegistrationFail(fail);
    
    // Логирование ошибки
    _logManager.AddErrorLog($"Ошибка регистрации заявки: {fail.Error}");
    
    // Уведомление пользователя
    this.GuiAsync(() => 
    {
        MessageBox.Show(this, 
            $"Не удалось зарегистрировать заявку: {fail.Error}", 
            "Ошибка регистрации", 
            MessageBoxButton.OK, 
            MessageBoxImage.Error);
    });
}
```