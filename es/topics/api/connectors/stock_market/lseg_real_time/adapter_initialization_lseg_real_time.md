# Inicialización del adaptador: LSEG Real-Time

El código siguiente muestra cómo inicializar [LsegRealTimeMessageAdapter](xref:StockSharp.LsegRealTime.LsegRealTimeMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsegRealTimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Secret = "<valor>".ToSecureString(),
	Address = "<valor>",
	StandbyAddress = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
