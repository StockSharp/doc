# Security lookup

![Screenshot: security lookup panel](../../../../images/gui_securitylookuppanel.png)

[SecurityLookupPanel](xref:StockSharp.Xaml.SecurityLookupPanel) - a security lookup panel. The code or a part of it is typed in the search box, and behind the extra filter button is a [Security](xref:StockSharp.BusinessEntities.Security) editor where the type, board, currency and expiration date are set.

The panel does not search anything itself \- on the search button or the enter key it raises the `Lookup` event with the filled filter. What to do next \- send the request to the connector or search the local storage \- is up to the application.

Below are code snippets showing its usage:

```xaml
<Window x:Class="Sample.LookupWindow"
	xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
	xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
	xmlns:xaml="http://schemas.stocksharp.com/xaml"
	Height="500" Width="400">
	<xaml:SecurityLookupPanel x:Name="LookupPanel" />
</Window>
```

```cs
// Send the lookup request to the connector
LookupPanel.Lookup += filter =>
{
	// The filter arrives filled in
	_connector.Subscribe(new Subscription(filter.ToLookupMessage()));
};

// Show the found securities in the table
_connector.SecurityReceived += (subscription, security) =>
	this.GuiAsync(() => SecurityGrid.Securities.Add(security));
```

## See also

[Service panels](../service_panels.md)
