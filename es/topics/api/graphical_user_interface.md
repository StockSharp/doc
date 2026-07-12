# Interfaz gráfica de usuario

## Componentes gráficos de S#

[S#](../api.md) incluye una gran cantidad de componentes gráficos propios. Los componentes se encuentran en los espacios de nombres [StockSharp.Xaml](xref:StockSharp.Xaml), [StockSharp.Xaml.Charting](xref:StockSharp.Xaml.Charting) y [StockSharp.Xaml.Diagram](xref:StockSharp.Xaml.Diagram). 

[S#](../api.md) dispone de diversos controles para: 

- buscar y seleccionar datos (instrumentos, portafolios, direcciones);
- crear órdenes;
- mostrar información bursátil y de otro tipo (operaciones, órdenes, transacciones, libros de órdenes, registros, etc.);
- construir gráficos.

Para acceder a los controles gráficos de [S#](../api.md) en el código XAML, debe definir los alias para el espacio de nombres correspondiente y usar estos alias en el código XAML. El siguiente ejemplo muestra cómo hacerlo:

```xaml
<Window x:Class="SampleSmartSMA.MainWindow"
		xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
		xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
		xmlns:loc="clr-namespace:StockSharp.Localization;assembly=StockSharp.Localization"
		xmlns:sx="clr-namespace:StockSharp.Xaml;assembly=StockSharp.Xaml"
		xmlns:charting="http://schemas.stocksharp.com/xaml"
		Title="{x:Static loc:LocalizedStrings.XamlStr570}" Height="700" Width="900">
	
	<Grid>
	</Grid>
</Window>
	
```
