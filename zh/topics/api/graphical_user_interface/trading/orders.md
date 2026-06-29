# 订单

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) 是用于显示订单和条件订单的表格。此外，该表格的上下文菜单包含用于订单操作的命令：订单注册、替换和取消。选择菜单项会分别生成事件：[OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering)、[OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) 或 [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling)。

![GUI OrderGrid](../../../../images/gui_ordergrid.png)

> [!TIP]
> 操作本身（注册、替换、取消）不会执行。相应的代码需要在事件处理程序中手动编写。

**主要成员**

- [OrderGrid.Orders](xref:StockSharp.Xaml.OrderGrid.Orders) - 订单列表。
- [OrderGrid.SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) - 已选择的订单。
- [OrderGrid.SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) - 已选择的订单。
- [OrderGrid.AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail))**(**[StockSharp.BusinessEntities.OrderFail](xref:StockSharp.BusinessEntities.OrderFail) 失败 **)** - 将订单注册错误信息添加到备注字段的方法。
- [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) - 订单注册事件（在选择相应的上下文菜单项后发生）。
- [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) - 订购替换事件（在选择相应的上下文菜单项后发生）。
- [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) - 订单取消事件（在选择相应的上下文菜单项后发生）。

下面是显示其用法的代码片段。代码示例取自 *Samples/01_Basic/03_Orders*。

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

## 通过订阅处理订单

处理订单的现代方法涉及使用订阅：

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

## 取消订单

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

## 处理订单注册和取消错误

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