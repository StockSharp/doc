# Inicialización del adaptador: Dukascopy JForex

El código siguiente muestra cómo inicializar [DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<valor>".ToSecureString(),
	UserName = "<valor>",
	BridgeJarPath = "<valor>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
