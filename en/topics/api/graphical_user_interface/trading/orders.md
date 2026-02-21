# Orders

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) is a table for displaying orders and conditional orders. In addition, the context menu of this table contains commands for operations with orders: registration, replacement, and cancellation of orders. Selecting a menu item generates events: [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering), [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering), or [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) respectively.

![GUI OrderGrid](../../../../images/gui_ordergrid.png)

> [!TIP]
> The operation itself (registration, replacement, cancellation) is not performed. The corresponding code needs to be written in the event handlers manually.

**Main Members**

- [OrderGrid.Orders](xref:StockSharp.Xaml.OrderGrid.Orders) - list of orders.
- [OrderGrid.SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) - selected order.
- [OrderGrid.SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) - selected orders.
- [OrderGrid.AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail))**(**[StockSharp.BusinessEntities.OrderFail](xref:StockSharp.BusinessEntities.OrderFail) fail **)** - method that adds an order registration error message to the comment field.
- [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) - order registration event (occurs after selecting the corresponding context menu item).
- [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) - order replacement event (occurs after selecting the corresponding context menu item).
- [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) - order cancellation event (occurs after selecting the corresponding context menu item).

Below are code fragments showing its use. The code example is taken from *Samples\/01\_Basic\/03\_Orders*.

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
	// Other code during connection...
	
	// Subscribe to the order received event
	_connector.OrderReceived += (subscription, order) => 
	{
		// Add orders to the OrderGrid table
		_ordersWindow.OrderGrid.Orders.TryAdd(order);
	};
	
	// To connect the connector
	_connector.Connect();
}
					
// Cancels all selected orders
private void OrderGrid_OnOrderCanceling(IEnumerable<Order> orders)
{
	// Iterate through selected orders and cancel each one
	foreach (var order in orders)
	{
		_connector.CancelOrder(order);
	}
}

// Opens an order editing window and performs replacement of the selected order
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

## Working with Orders through Subscriptions

The modern approach to working with orders involves using subscriptions:

```cs
// Subscribe to the order received event
_connector.OrderReceived += OnOrderReceived;

// Order received handler
private void OnOrderReceived(Subscription subscription, Order order)
{
	// Check if the order belongs to the subscription we're interested in
	if (subscription == _ordersSubscription)
	{
		// Add the order to the table
		_ordersWindow.OrderGrid.Orders.TryAdd(order);
		
		// Additional order processing
		Console.WriteLine($"Order received: {order.TransactionId}, Status: {order.State}");
		
		// If the order is in a final state, update the UI
		if (order.State == OrderStates.Done || order.State == OrderStates.Failed)
		{
			this.GuiAsync(() => {
				// Update interface for completed orders
			});
		}
	}
}
```

## Canceling Orders

```cs
// Modern approach to order cancellation
private void CancelOrder(Order order)
{
	try
	{
		_connector.CancelOrder(order);
		
		// Log the action
		_logManager.AddInfoLog($"Order cancellation command sent {order.TransactionId}");
	}
	catch (Exception ex)
	{
		_logManager.AddErrorLog($"Error when canceling order: {ex.Message}");
	}
}

// Mass cancellation of orders
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

## Handling Order Registration and Cancellation Errors

```cs
// Subscribe to order registration failures
_connector.OrderRegisterFailReceived += OnOrderRegisterFailed;

// Order registration failure handler
private void OnOrderRegisterFailed(Subscription subscription, OrderFail fail)
{
	// Add error information to OrderGrid
	_ordersWindow.OrderGrid.AddRegistrationFail(fail);
	
	// Log the error
	_logManager.AddErrorLog($"Order registration error: {fail.Error}");
	
	// Notify the user
	this.GuiAsync(() => 
	{
		MessageBox.Show(this, 
			$"Failed to register order: {fail.Error}", 
			"Registration Error", 
			MessageBoxButton.OK, 
			MessageBoxImage.Error);
	});
}
```