# Inicialización del adaptador ThetaData

El siguiente código muestra cómo inicializar [ThetaDataMessageAdapter](xref:StockSharp.ThetaData.ThetaDataMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ThetaDataMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
