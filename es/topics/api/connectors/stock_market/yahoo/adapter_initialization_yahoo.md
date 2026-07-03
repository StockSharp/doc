# Inicialización del adaptador Yahoo

El siguiente código muestra cómo inicializar [YahooMessageAdapter](xref:StockSharp.Yahoo.YahooMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
