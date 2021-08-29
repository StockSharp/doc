# Graphical user interface

This topic is designed for traders who develop graphical trading programs via [S\#](StockSharpAbout.md), but not enough familiar with the basics of GUI programming in [.NET](https://en.wikipedia.org/wiki/.NET_Framework).

There is a special technology [WPF](https://en.wikipedia.org/wiki/Windows_Presentation_Foundation) for the GUI building in [.NET](https://en.wikipedia.org/wiki/.NET_Framework) (before there was the [WinForms](https://en.wikipedia.org/wiki/WinForms) technology that significantly was inferior by the graphics capabilities). To create graphical elements a special declarative language [XAML](https://msdn.microsoft.com/library/hh700354.aspx) is used in this technology.

The main limitation of the Windows visual API is that it is impossible to access to the window elements from another thread. This is due to the Windows architectural limitations (described in more detail here [https:\/\/msdn.microsoft.com\/en\-US\/library\/ms741870.aspx](https://msdn.microsoft.com/en-US/library/ms741870.aspx)). The [IConnector](xref:StockSharp.BusinessEntities.IConnector) interface implementations work in multithreaded mode to improve productivity. Therefore, by subscribing to an event, for example, [Connector.NewSecurity](xref:StockSharp.Algo.Connector.NewSecurity), it is impossible to directly display the data in a user window. To do this, in is necessary to perform the synchronizing operation via the special [Dispatcher](https://msdn.microsoft.com/library/system.windows.threading.dispatcher.aspx) object, which controls the queue of the thread working elements.

Here is a simple example of how this is done:

```cs
// the BeginInvoke method must be invoked for access to any UI compenents in market-data handles
_connector.NewSecurity += security => this.Dispatcher.BeginInvoke((Action)(() => this.Security.ItemsSource = _connector.Securities));
```

[S\#](StockSharpAbout.md) already has special methods that hide the Dispatcher use and simplify coding: 

```cs
// the same as a prev example but uses short notation
_connector.NewSecurity += security => this.GuiSync(() => this.Security.ItemsSource = _connector.Securities);
```

## Graphical components of S\#

[S\#](StockSharpAbout.md) includes a large number of its own graphical components, as well as a free library for Wpf [Xceed.Wpf.Toolkit](https://wpftoolkit.codeplex.com/). Its own graphical components are placed in the [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) and [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram) namespaces. Some specific components are in the connectors namespaces, for example the combined list [OpenECryAddressComboBox](xref:StockSharp.OpenECry.Xaml.OpenECryAddressComboBox) for the [OpenECry](OEC.md) server address selecting. 

[S\#](StockSharpAbout.md) has a variety of controls for: 

- search and select data (instruments, portfolios, addresses);
- the orders creation;
- display exchange and other information (trades, orders, transactions, order books, logs, etc.);
- charts plotting.

To access the [S\#](StockSharpAbout.md) graphical controls in the XAML code it is necessary to define the aliases for the corresponding namespace and to use these aliases in the XAML code. How to do this is shown in the following example: 

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
