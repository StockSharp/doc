# Tabelle

Die Komponente [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) dient zur tabellarischen Anzeige von Finanzinformationen (Level1-Felder) und deren Änderungen in Bezug auf Instrumente. Die Komponente ermöglicht die Auswahl eines oder mehrerer Instrumente.

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

**Haupteigenschaften**

- [SecurityGrid.Securities](xref:StockSharp.Xaml.SecurityGrid.Securities) - Liste der Instrumente.
- [SecurityGrid.SelectedSecurity](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurity) - ausgewähltes Instrument.
- [SecurityGrid.SelectedSecurities](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurities) - Liste der ausgewählten Instrumente.
- [SecurityGrid.MarketDataProvider](xref:StockSharp.Xaml.SecurityGrid.MarketDataProvider) - Marktdatenanbieter.

Beachten Sie, dass für die Anzeige von Änderungen in Marktdaten ein Marktdatenanbieter angegeben werden muss.

Unten sehen Sie ein Codefragment zur Verwendung.

In der Abbildung ist die Komponente [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) in der grafischen Komponente [SecurityPicker](picker.md) dargestellt.

```xaml
<Window x:Class="SecurityGridSample.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		Title="MainWindow" Height="350" Width="525">
	<Grid>
		<sx:SecurityGrid x:Name="SecurityGrid"/>
	</Grid>
</Window>
	  				
```
```cs
private readonly Connector _connector = new Connector();
SecurityGrid.MarketDataProvider = _connector;
..........................
_connector.SecurityReceived += (sub, security) =>
{
	SecurityGrid.Securities.Add(security);
};
..........................
private void ColumnsFilter()
{
	string[]  columns = { "Board", "BestAsk.Price", "BestAsk.Volume" };
	
	foreach (var column in SecurityGrid.Columns)
	{
		column.Visibility = columns.Contains(column.SortMemberPath) ? Visibility.Visible : Visibility.Collapsed;
	}
}
				
```
