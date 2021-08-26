# Ввод стоп\-заявки

> [!TIP]
> Данный контролл устарел, вместо него следует использовать [OrderWindow](../api/StockSharp.Xaml.OrderWindow.html). Как описано в пункте [Ввод новой заявки](GuiOrderWindow.md).

[OrderConditionalWindow](../api/StockSharp.Xaml.OrderConditionalWindow.html) \- окно для создания условной заявки. 

![GUI OrderConditionalWindow](~/images/GUI_OrderConditionalWindow.png)

**Основные свойства**

- [Portfolios](../api/StockSharp.Xaml.OrderConditionalWindow.Portfolios.html) \- список портфелей. 
- [SecurityProvider](../api/StockSharp.Xaml.OrderConditionalWindow.SecurityProvider.html) \- поставщик информации об инструментах. 
- [MarketDataProvider](../api/StockSharp.Xaml.OrderConditionalWindow.MarketDataProvider.html) \- поставщик рыночных данных. 
- [Adapter](../api/StockSharp.Xaml.OrderConditionalWindow.Adapter.html) \- адаптер сообщений. 
- [Order](../api/StockSharp.Xaml.OrderConditionalWindow.Order.html) \- созданная заявка. 

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```cs
...
private readonly Connector \_connector \= new Connector();
...
private void NewStopOrderClick(object sender, RoutedEventArgs e)
{
	var newOrder \= new OrderConditionalWindow
	{
		Order \= new Order
		{
			Security \= SecurityPicker.SelectedSecurity,
			Type \= OrderTypes.Conditional,
			ExpiryDate \= DateTime.Today
		},
		SecurityProvider \= \_connector,
		MarketDataProvider \= \_connector,
		Portfolios \= new PortfolioDataSource(\_connector),
		Adapter \= \_connector.Adapter
	};
	if (newOrder.ShowModal(this))
		\_connector.RegisterOrder(newOrder.Order);
}
              		
	  				
```
