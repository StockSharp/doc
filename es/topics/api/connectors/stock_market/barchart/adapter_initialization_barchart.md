# Inicialización del adaptador BarChart

El siguiente código muestra cómo inicializar [BarChartMessageAdapter](xref:StockSharp.BarChart.BarChartMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BarChartMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su token>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
