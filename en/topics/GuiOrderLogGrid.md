# Order log

![GUI orderlog](../images/GUI_orderlog.png)

[OrderLogGrid](xref:StockSharp.Xaml.OrderLogGrid) is a graphical component to display the order log ( [OrderLogItem](xref:StockSharp.BusinessEntities.OrderLogItem)). 

**Main properties and methods**

- [LogItems](xref:StockSharp.Xaml.OrderLogGrid.LogItems) \- list of items of the order log.
- [SelectedLogItem](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItem) \- selected item of the order log.
- [SelectedLogItems](xref:StockSharp.Xaml.OrderLogGrid.SelectedLogItems) \- selected items of the order log.

Below is the code snippet with its use. The code example is taken from *Samples\/ITCH\/SampleITCH.*

```xaml
Window x:Class="SampleITCH.OrdersLogWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
        xmlns:xaml="http://schemas.stocksharp.com/xaml"
        Title="{x:Static loc:LocalizedStrings.OrderLog}" Height="750" Width="900">
	<xaml:OrderLogGrid x:Name="OrderLogGrid" x:FieldModifier="public" />
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
	.................................................
		
	_connector.NewOrderLogItem += _orderLogWindow.OrderLogGrid.LogItems.Add;
	.................................................
}
	  				
```
