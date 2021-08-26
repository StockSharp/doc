# Orders

[OrderGrid](../api/StockSharp.Xaml.OrderGrid.html) \- the table to display orders. In addition, the context menu of this table contains commands for operations with orders: registration, replacement and cancellation of orders. Selecting a menu item leads to the generation of events: [OrderGrid.OrderRegistering](../api/StockSharp.Xaml.OrderGrid.OrderRegistering.html), [OrderGrid.OrderReRegistering](../api/StockSharp.Xaml.OrderGrid.OrderReRegistering.html) or [OrderGrid.OrderCanceling](../api/StockSharp.Xaml.OrderGrid.OrderCanceling.html) respectively.

![GUI OrderGrid](~/images/GUI_OrderGrid.png)

> [!TIP]
> The operation itself (registration, replacement, cancellation) is not performed. The appropriate code must be written in the event handlers independently.

**Main members**

- [Orders](../api/StockSharp.Xaml.OrderGrid.Orders.html) \- the list of orders.
- [SelectedOrder](../api/StockSharp.Xaml.OrderGrid.SelectedOrder.html) \- the selected order.
- [SelectedOrders](../api/StockSharp.Xaml.OrderGrid.SelectedOrders.html) \- \- selected orders.
- [AddRegistrationFail](../api/StockSharp.Xaml.OrderGrid.AddRegistrationFail.html) \- the method that adds a message about order registration error in the comment field.
- [OrderRegistering](../api/StockSharp.Xaml.OrderGrid.OrderRegistering.html) \- the order registration event (occurs after selecting the appropriate item in the context menu).
- [OrderReRegistering](../api/StockSharp.Xaml.OrderGrid.OrderReRegistering.html) \- the order replacement event (occurs after selecting the appropriate item in the context menu).
- [OrderCanceling](../api/StockSharp.Xaml.OrderGrid.OrderCanceling.html) \- the order cancellation (occurs after selecting the appropriate item in the context menu).

Below is the code snippet with its use. The code example is taken from *Samples\/InteractiveBrokers\/SampleIB*. 

```xaml
\<Window x:Class\="Sample.OrdersWindow"
    xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
    xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
    xmlns:loc\="clr\-namespace:StockSharp.Localization;assembly\=StockSharp.Localization"
    xmlns:xaml\="http:\/\/schemas.stocksharp.com\/xaml"
    Title\="{x:Static loc:LocalizedStrings.Orders}" Height\="410" Width\="930"\>
	\<xaml:OrderGrid x:Name\="OrderGrid" x:FieldModifier\="public" OrderCanceling\="OrderGrid\_OnOrderCanceling" OrderReRegistering\="OrderGrid\_OnOrderReRegistering" \/\>
\<\/Window\>
	  				
```
```cs
private readonly Connector \_connector \= new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
 	.......................................	
	\_connector.NewOrder +\= order \=\> \_ordersWindow.OrderGrid.Orders.Add(order);
	
	\_connector.NewStopOrder +\= order \=\> \_stopOrdersWindow.OrderGrid.Orders.Add(order);
	.......................................			
}
              	
private void OrderGrid\_OnOrderCanceling(IEnumerable\<Order\> orders)
{
	orders.ForEach(\_connector.CancelOrder);
}
private void OrderGrid\_OnOrderReRegistering(Order order)
{
	var window \= new OrderWindow
	{
		Title \= LocalizedStrings.Str2976Params.Put(order.TransactionId),
		SecurityProvider \= \_connector,
		MarketDataProvider \= \_connector,
		Portfolios \= new PortfolioDataSource(\_connector),
		Order \= order.ReRegisterClone(newVolume: order.Balance)
	};
	if (window.ShowModal(this))
		\_connector.ReRegisterOrder(order, window.Order);
}
	  				
```
