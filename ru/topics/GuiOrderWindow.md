# Ввод новой заявки

[OrderWindow](../api/StockSharp.Xaml.OrderWindow.html) \- окно для создания заявки. 

![GUI OrderWindow](~/images/GUI_OrderWindow.png)

Если подключение поддерживает выставление условной заявки (стоп\-лосс, тейк\-профи), то в этом окне можно выставить условную заявку с расширенными условиями установив флаг **Расширенные условия**.

**Основные свойства**

- [Portfolios](../api/StockSharp.Xaml.OrderWindow.Portfolios.html) \- список портфелей.
- [MarketDataProvider](../api/StockSharp.Xaml.OrderWindow.MarketDataProvider.html) \- поставщик рыночных данных.
- [SecurityProvider](../api/StockSharp.Xaml.OrderWindow.SecurityProvider.html) \- поставщик информации об инструментах.
- [Order](../api/StockSharp.Xaml.OrderWindow.Order.html) \- созданная заявка.

Ниже показаны фрагменты кода с его использованием. Пример кода взят из *Samples\/Common\/SampleConnection*. 

```cs
...
private readonly Connector \_connector \= new Connector();
...
private void NewOrderClick(object sender, RoutedEventArgs e)
{
	var newOrder \= new OrderWindow
	{
		Order \= new Order { Security \= SecurityPicker.SelectedSecurity },
		SecurityProvider \= \_connector,
		MarketDataProvider \= \_connector,
		Portfolios \= new PortfolioDataSource(\_connector),
	};
	if (newOrder.ShowModal(this))
		\_connector.RegisterOrder(newOrder.Order);
}
              		
	  				
```
