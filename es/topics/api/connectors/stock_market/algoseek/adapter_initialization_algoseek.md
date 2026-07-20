# Inicialización del adaptador AlgoSeek

El siguiente código muestra cómo inicializar [AlgoSeekMessageAdapter](xref:StockSharp.AlgoSeek.AlgoSeekMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlgoSeekMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
