# Inicialización del adaptador Xignite

El siguiente código muestra cómo inicializar [XigniteMessageAdapter](xref:StockSharp.Xignite.XigniteMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
