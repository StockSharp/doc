# New order creation

[OrderWindow](xref:StockSharp.Xaml.OrderWindow) \- window for creating an order. 

![GUI OrderWindow](../../../../images/gui_orderwindow.png)

If the connection supports the registering a conditional order (stop\-loss, take\-profit), then in this window you can register a conditional order with advanced conditions by setting the **Advanced conditions** flag.

**Basic properties**

- [OrderWindow.Portfolios](xref:StockSharp.Xaml.OrderWindow.Portfolios) \- list of portfolios.
- [OrderWindow.MarketDataProvider](xref:StockSharp.Xaml.OrderWindow.MarketDataProvider) \- market data provider.
- [OrderWindow.SecurityProvider](xref:StockSharp.Xaml.OrderWindow.SecurityProvider) \- security information provider.
- [OrderWindow.Order](xref:StockSharp.Xaml.OrderWindow.Order) \- created order.

Code snippets using it are shown below. Sample code taken from *Samples\/Common\/SampleConnection*. 

```cs
...
private readonly Connector _connector = new Connector();
...
private void NewOrderClick(object sender, RoutedEventArgs e)
{
	var newOrder = new OrderWindow
	{
		Order = new Order { Security = SecurityPicker.SelectedSecurity },
		SecurityProvider = _connector,
		MarketDataProvider = _connector,
		Portfolios = new PortfolioDataSource(_connector),
	};
	if (newOrder.ShowModal(this))
		_connector.RegisterOrder(newOrder.Order);
}
              		
	  				
```
