# Graphical user interface

## Graphical components of S\#

[S\#](../api.md) includes a large number of its own graphical components. The components are placed in the [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) and [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram) namespaces. 

[S\#](../api.md) has a variety of controls for: 

- search and select data (instruments, portfolios, addresses);
- the orders creation;
- display exchange and other information (trades, orders, transactions, order books, logs, etc.);
- charts plotting.

To access the [S\#](../api.md) graphical controls in the XAML code it is necessary to define the aliases for the corresponding namespace and to use these aliases in the XAML code. How to do this is shown in the following example: 

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		xmlns:ss="clr-namespace:StockSharp.SmartCom.Xaml;assembly=StockSharp.SmartCom"
		xmlns:charting="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
	
	<Grid>
	</Grid>
</Window>
	
```
