# Ordens

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) é uma tabela para apresentar ordens e ordens condicionais. Além disso, o menu de contexto desta tabela contém comandos para operações com ordens: registo, substituição e cancelamento de ordens. A seleção de um item de menu gera os eventos: [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering), [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) ou [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling), respetivamente.

![GUI OrderGrid](../../../../images/gui_ordergrid.png)

> [!TIP]
> A operação em si (registo, substituição, cancelamento) não é executada. O código correspondente tem de ser escrito manualmente nos manipuladores de eventos.

**Membros principais**

- [OrderGrid.Orders](xref:StockSharp.Xaml.OrderGrid.Orders) - lista de ordens.
- [OrderGrid.SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) - ordem selecionada.
- [OrderGrid.SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) - ordens selecionadas.
- [OrderGrid.AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail))**(**[StockSharp.BusinessEntities.OrderFail](xref:StockSharp.BusinessEntities.OrderFail) fail **)** - método que adiciona uma mensagem de erro de registo de ordem ao campo de comentário.
- [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) - evento de registo de ordem (ocorre após selecionar o item correspondente do menu de contexto).
- [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) - evento de substituição de ordem (ocorre após selecionar o item correspondente do menu de contexto).
- [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) - evento de cancelamento de ordem (ocorre após selecionar o item correspondente do menu de contexto).

Abaixo estão fragmentos de código que mostram a sua utilização. O exemplo de código foi retirado de *Samples\/01\_Basic\/03\_Orders*.

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

## Trabalhar com ordens através de subscrições

A abordagem moderna para trabalhar com ordens envolve a utilização de subscrições:

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

## Cancelar ordens

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

## Tratar erros de registo e cancelamento de ordens

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
