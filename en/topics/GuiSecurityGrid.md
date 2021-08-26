# Table

The [SecurityGrid](../api/StockSharp.Xaml.SecurityGrid.html) component is designed to display financial information (level1 fields) and its changes relating to instruments in a tabular form. The component allows you to select one or more instruments. 

![GUI SecurityPicker2](../images/GUI_SecurityPicker2.png)

**Main properties**

- [Securities](../api/StockSharp.Xaml.SecurityGrid.Securities.html) \- the list of instruments.
- [SelectedSecurity](../api/StockSharp.Xaml.SecurityGrid.SelectedSecurity.html) \- the selected instrument.
- [SelectedSecurities](../api/StockSharp.Xaml.SecurityGrid.SelectedSecurities.html) \- the list of selected instruments.
- [MarketDataProvider](../api/StockSharp.Xaml.SecurityGrid.MarketDataProvider.html) \- the provider of market data.

Please note that for the display of changes in market information, you must specify a provider of market data. 

Below is the code snippet with its use. 

In the figure, the [SecurityGrid](../api/StockSharp.Xaml.SecurityGrid.html) component is shown in the [SecurityPicker](GuiSecurityPicker.md) graphical component. 

```xaml
\<Window x:Class\="SecurityGridSample.MainWindow"
        xmlns\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml\/presentation"
        xmlns:x\="http:\/\/schemas.microsoft.com\/winfx\/2006\/xaml"
        xmlns:sx\="clr\-namespace:StockSharp.Xaml;assembly\=StockSharp.Xaml"
        Title\="MainWindow" Height\="350" Width\="525"\>
    \<Grid\>
        \<sx:SecurityGrid x:Name\="SecurityGrid"\/\>
    \<\/Grid\>
\<\/Window\>
	  				
```
```cs
private readonly Connector \_connector \= new Connector();
SecurityGrid.MarketDataProvider \= \_connector;
..........................
\_connector.NewSecurity +\= security \=\>
{
	SecurityGrid.Securities.Add(security);
};
..........................
private void ColumnsFilter()
{
	string\[\]  columns \= { "Board", "BestAsk.Price", "BestAsk.Volume" };
	
	foreach (var column in SecurityGrid.Columns)
	{
		column.Visibility \= columns.Contains(column.SortMemberPath) ? Visibility.Visible : Visibility.Collapsed;
	}
}
              
```
