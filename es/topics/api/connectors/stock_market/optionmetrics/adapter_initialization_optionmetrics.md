# Inicialización del adaptador OptionMetrics IvyDB

El siguiente código muestra cómo inicializar [OptionMetricsMessageAdapter](xref:StockSharp.OptionMetrics.OptionMetricsMessageAdapter) y enviarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OptionMetricsMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
