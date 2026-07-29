# Aufträge

[OrderGrid](xref:StockSharp.Xaml.OrderGrid) ist eine Tabelle zur Anzeige von Aufträgen und bedingten Aufträgen. Außerdem enthält das Kontextmenü dieser Tabelle Befehle für Operationen mit Aufträgen: Registrierung, Änderung und Stornierung von Aufträgen. Die Auswahl eines Menüeintrags erzeugt die Ereignisse [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering), [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) bzw. [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling).

![Aufträge – Bildschirmfoto](../../../../images/gui_ordergrid.png)

> [!TIP]
> Die Operation selbst (Registrierung, Änderung, Stornierung) wird nicht ausgeführt. Der entsprechende Code muss manuell in den Ereignishandlern geschrieben werden.

**Wichtigste Member**

- [OrderGrid.Orders](xref:StockSharp.Xaml.OrderGrid.Orders) - Liste der Aufträge.
- [OrderGrid.SelectedOrder](xref:StockSharp.Xaml.OrderGrid.SelectedOrder) - ausgewählter Auftrag.
- [OrderGrid.SelectedOrders](xref:StockSharp.Xaml.OrderGrid.SelectedOrders) - ausgewählte Aufträge.
- [OrderGrid.AddRegistrationFail](xref:StockSharp.Xaml.OrderGrid.AddRegistrationFail(StockSharp.BusinessEntities.OrderFail))**(**[StockSharp.BusinessEntities.OrderFail](xref:StockSharp.BusinessEntities.OrderFail) fail **)** - Methode, die eine Fehlermeldung zur Auftragsregistrierung zum Kommentarfeld hinzufügt.
- [OrderGrid.OrderRegistering](xref:StockSharp.Xaml.OrderGrid.OrderRegistering) - Ereignis zur Auftragsregistrierung (tritt nach Auswahl des entsprechenden Kontextmenüeintrags auf).
- [OrderGrid.OrderReRegistering](xref:StockSharp.Xaml.OrderGrid.OrderReRegistering) - Ereignis zur Auftragsänderung (tritt nach Auswahl des entsprechenden Kontextmenüeintrags auf).
- [OrderGrid.OrderCanceling](xref:StockSharp.Xaml.OrderGrid.OrderCanceling) - Ereignis zur Auftragsstornierung (tritt nach Auswahl des entsprechenden Kontextmenüeintrags auf).

Unten sind Codefragmente für die Verwendung gezeigt. Das Codebeispiel stammt aus *Samples\/01\_Basic\/03\_Orders*.

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
	// Sonstiger Code während der Verbindung...

	// Ereignis für empfangene Aufträge abonnieren
	_connector.OrderReceived += (subscription, order) =>
	{
		// Aufträge zur Tabelle OrderGrid hinzufügen
		_ordersWindow.OrderGrid.Orders.TryAdd(order);
	};

	// Connector verbinden
	_connector.Connect();
}

// Storniert alle ausgewählten Aufträge
private void OrderGrid_OnOrderCanceling(IEnumerable<Order> orders)
{
	// Durch ausgewählte Aufträge iterieren und jeden stornieren
	foreach (var order in orders)
	{
		_connector.CancelOrder(order);
	}
}

// Öffnet ein Fenster zum Bearbeiten des Auftrags und führt die Änderung des ausgewählten Auftrags aus
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

## Arbeiten mit Aufträgen über Abonnements

Der moderne Ansatz für die Arbeit mit Aufträgen verwendet Abonnements:

```cs
// Ereignis für empfangene Aufträge abonnieren
_connector.OrderReceived += OnOrderReceived;

// Handler für empfangene Aufträge
private void OnOrderReceived(Subscription subscription, Order order)
{
	// Prüfen, ob der Auftrag zum für uns relevanten Abonnement gehört
	if (subscription == _ordersSubscription)
	{
		// Auftrag zur Tabelle hinzufügen
		_ordersWindow.OrderGrid.Orders.TryAdd(order);

		// Zusätzliche Auftragsverarbeitung
		Console.WriteLine($"Auftrag empfangen: {order.TransactionId}, Status: {order.State}");

		// Wenn der Auftrag in einem finalen Zustand ist, UI aktualisieren
		if (order.State == OrderStates.Done || order.State == OrderStates.Failed)
		{
			this.GuiAsync(() => {
				// Oberfläche für abgeschlossene Aufträge aktualisieren
			});
		}
	}
}
```

## Aufträge stornieren

```cs
// Moderner Ansatz zur Auftragsstornierung
private void CancelOrder(Order order)
{
	try
	{
		_connector.CancelOrder(order);

		// Aktion protokollieren
		_logManager.AddInfoLog($"Befehl zur Auftragsstornierung gesendet: {order.TransactionId}");
	}
	catch (Exception ex)
	{
		_logManager.AddErrorLog($"Fehler beim Stornieren des Auftrags: {ex.Message}");
	}
}

// Massenstornierung von Aufträgen
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

## Fehler bei Auftragsregistrierung und -stornierung behandeln

```cs
// Fehler bei der Auftragsregistrierung abonnieren
_connector.OrderRegisterFailReceived += OnOrderRegisterFailed;

// Handler für fehlgeschlagene Auftragsregistrierung
private void OnOrderRegisterFailed(Subscription subscription, OrderFail fail)
{
	// Fehlerinformationen zu OrderGrid hinzufügen
	_ordersWindow.OrderGrid.AddRegistrationFail(fail);

	// Fehler protokollieren
	_logManager.AddErrorLog($"Fehler bei der Auftragsregistrierung: {fail.Error}");

	// Benutzer benachrichtigen
	this.GuiAsync(() =>
	{
		MessageBox.Show(this,
			$"Auftrag konnte nicht registriert werden: {fail.Error}",
			"Registrierungsfehler",
			MessageBoxButton.OK,
			MessageBoxImage.Error);
	});
}
```
