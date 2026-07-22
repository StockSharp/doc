# Inicialización del adaptador: DukasCopy Live

El código siguiente muestra cómo inicializar [DukasCopyLiveMessageAdapter](xref:StockSharp.DukasCopyLive.DukasCopyLiveMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyLiveMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	Login = "<valor>",
	BridgeJarPath = "<valor>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
