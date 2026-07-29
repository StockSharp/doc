# Inicialización del adaptador de historial de Gate.io

El código siguiente muestra cómo inicializar [GateIOHistoryMessageAdapter](xref:StockSharp.GateIOHistory.GateIOHistoryMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new();
...
var messageAdapter = new GateIOHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
