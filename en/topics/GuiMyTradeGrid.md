# Own trades

[MyTradeGrid](../api/StockSharp.Xaml.MyTradeGrid.html) \- a table to display own trades. 

![GUI MytradeGrid](../images/GUI_MytradeGrid.png)

**Main members**

- [Trades](../api/StockSharp.Xaml.MyTradeGrid.Trades.html) \- \- list of trades.
- [SelectedTrade](../api/StockSharp.Xaml.MyTradeGrid.SelectedTrade.html) \- the selected trade.
- [SelectedTrades](../api/StockSharp.Xaml.MyTradeGrid.SelectedTrades.html) \- selected trades.

Below is the code snippet with its use. The code example is taken from *Samples\/InteractiveBrokers\/SampleIB.*

```xaml
<Window x:Class="Sample.MyTradesWindow"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
    xmlns:xaml="http://schemas.stocksharp.com/xaml"
    Title="{x:Static loc:LocalizedStrings.MyTrades}" Height="284" Width="644">
	<xaml:MyTradeGrid x:Name="TradeGrid" x:FieldModifier="public" />
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();
private void ConnectClick(object sender, RoutedEventArgs e)
{
        ...............................................
		_connector.NewMyTrade += trade => _myTradesWindow.TradeGrid.Trades.Add(trade);
			
		...............................................
}
	  				
```
