# Inicialización del adaptador cTrader

El siguiente código muestra cómo inicializar [cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
