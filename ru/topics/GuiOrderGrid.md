# Заявки

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) \- таблица для отображения заявок и условных заявок. Кроме того контекстное меню этой таблицы содержит команды для операций с заявками: регистрация, замена и отмена заявок. Выбор пункта меню приводит к генерации событий: [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering), [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) или [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) соответственно.

![GUI OrderGrid](../images/GUI_OrderGrid.png)

> [!TIP]
> Сама операция (регистрация, замена, отмена) не выполняется. Соответствующий код нужно прописывать в обработчиках событий самостоятельно.

**Основные члены**

- [Orders](xref:StockSharp.Xaml.OrderGrid.Orders) \- список заявок.
- [SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) \- выбранная заявка.
- [SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) \- выбранные заявки.
- [AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail)) \- метод, который добавляет сообщение об ошибке регистрации заявки в поле комментария.
- [OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) \- событие регистрации заявки (возникает после выбора соответствующего пункта контекстного меню).
- [OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) \- событие замены заявки (возникает после выбора соответствующего пункта контекстного меню).
- [OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) \- событие отмены заявки (возникает после выбора соответствующего пункта контекстного меню).

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```xaml
<Window x:Class="Sample.OrdersWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="{x:Static loc:LocalizedStrings.Orders}" Height="410" Width="930">
	<xaml:OrderGrid x:Name="OrderGrid" x:FieldModifier="public" OrderCanceling="OrderGrid_OnOrderCanceling" OrderReRegistering="OrderGrid_OnOrderReRegistering" />
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
 	.......................................	
	// Добавляем заявки в таблицу OrderGrid
	_connector.NewOrder += order => _ordersWindow.OrderGrid.Orders.Add(order);
	
	// Добавляем стоп-заявки в таблицу OrderGrid
	_connector.NewStopOrder += order => _stopOrdersWindow.OrderGrid.Orders.Add(order);
	.......................................			
}
              	
// Удаляет все выбранные заявки
private void OrderGrid_OnOrderCanceling(IEnumerable<Order> orders)
{
	orders.ForEach(_connector.CancelOrder);
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
