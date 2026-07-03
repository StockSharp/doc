# Inicialización del adaptador Tradier

El siguiente código muestra cómo inicializar [TradierMessageAdapter](xref:StockSharp.Tradier.TradierMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
