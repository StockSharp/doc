# Inicialización del adaptador: Databento

El código siguiente muestra cómo inicializar [DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<valor>".ToSecureString(),
	Dataset = "<valor>",
	LiveAddress = "<valor>",
	HistoricalAddress = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
