# Order book

![GUI MarketDepthControl](../../../../images/gui_marketdepthcontrol.png)

[MarketDepthControl](xref:StockSharp.Xaml.MarketDepthControl) \- is a graphical component for displaying an order book. The component allows you to display quotes and own orders. 

**Main properties and methods**

- [MarketDepthControl.MaxDepth](xref:StockSharp.Xaml.MarketDepthControl.MaxDepth) \- the depth of an order book.
- [MarketDepthControl.IsBidsOnTop](xref:StockSharp.Xaml.MarketDepthControl.IsBidsOnTop) \- to display purchases on top.
- [MarketDepthControl.UpdateFormat](xref:StockSharp.Xaml.MarketDepthControl.UpdateFormat(StockSharp.BusinessEntities.Security))**(**[StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** \- to update the format for displaying prices and volumes using the instrument.
- [MarketDepthControl.ProcessOrder](xref:StockSharp.Xaml.MarketDepthControl.ProcessOrder(StockSharp.BusinessEntities.Order,System.Decimal,System.Decimal,StockSharp.Messages.OrderStates))**(**[StockSharp.BusinessEntities.Order](xref:StockSharp.BusinessEntities.Order) order, [System.Decimal](xref:System.Decimal) price, [System.Decimal](xref:System.Decimal) balance, [StockSharp.Messages.OrderStates](xref:StockSharp.Messages.OrderStates) state **)** \- to process an order.
- [MarketDepthControl.UpdateDepth](xref:StockSharp.Xaml.MarketDepthControl.UpdateDepth(StockSharp.Messages.IOrderBookMessage,StockSharp.BusinessEntities.Security))**(**[StockSharp.Messages.IOrderBookMessage](xref:StockSharp.Messages.IOrderBookMessage) message, [StockSharp.BusinessEntities.Security](xref:StockSharp.BusinessEntities.Security) security **)** \- to refresh an order book with a message.

Below is the code snippet with its use. The code example is taken from *Samples\/BarChart\/SampleBarChart*. 

```xaml
<Window x:Class="SampleBarChart.QuotesWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="QuotesWindow" Height="600" Width="280">
	<xaml:MarketDepthControl x:Name="DepthCtrl" x:FieldModifier="public" />
</Window>
	  				
```
```cs
private void ConnectorOnMarketDepthsChanged(IEnumerable<MarketDepth> depths)
{
	foreach (var depth in depths)
	{
		var wnd = _quotesWindows.TryGetValue(depth.Security);
		if (wnd != null)
			wnd.DepthCtrl.UpdateDepth(depth);
	}
}
	  				
```
