# New stop order creation

[OrderConditionalWindow](../api/StockSharp.Xaml.OrderConditionalWindow.html) \- the window for creating a conditional order. 

![GUI OrderConditionalWindow](~/images/GUI_OrderConditionalWindow.png)

**Main properties**

- [Portfolios](../api/StockSharp.Xaml.OrderConditionalWindow.Portfolios.html) \- the list of portfolios. 
- [SecurityProvider](../api/StockSharp.Xaml.OrderConditionalWindow.SecurityProvider.html) \- provider of information about instruments. 
- [MarketDataProvider](../api/StockSharp.Xaml.OrderConditionalWindow.MarketDataProvider.html) \- provider of market data. 
- [Adapter](../api/StockSharp.Xaml.OrderConditionalWindow.Adapter.html) \- message adapter. 
- [Order](../api/StockSharp.Xaml.OrderConditionalWindow.Order.html) \- the created order. 

Below is the code snippet with its use. The code example is taken from *Samples\/InteractiveBrokers\/SampleIB*. 

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
