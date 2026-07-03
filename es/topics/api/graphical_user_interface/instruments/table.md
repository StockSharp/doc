# Tabla

El componente [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) está diseñado para mostrar información financiera (campos level1) y sus cambios relacionados con instrumentos en forma tabular. El componente permite seleccionar uno o varios instrumentos. 

![GUI SecurityPicker2](../../../../images/gui_securitypicker2.png)

**Propiedades principales**

- [SecurityGrid.Securities](xref:StockSharp.Xaml.SecurityGrid.Securities) - lista de instrumentos.
- [SecurityGrid.SelectedSecurity](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurity) - instrumento seleccionado.
- [SecurityGrid.SelectedSecurities](xref:StockSharp.Xaml.SecurityGrid.SelectedSecurities) - lista de instrumentos seleccionados.
- [SecurityGrid.MarketDataProvider](xref:StockSharp.Xaml.SecurityGrid.MarketDataProvider) - proveedor de datos de mercado.

Tenga en cuenta que para mostrar cambios en la información de mercado debe especificar un proveedor de datos de mercado. 

A continuación se muestra el fragmento de código con su uso. 

En la figura, el componente [SecurityGrid](xref:StockSharp.Xaml.SecurityGrid) se muestra dentro del componente gráfico [SecurityPicker](picker.md). 

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

