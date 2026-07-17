# Inicialización del adaptador: Mirae Asset Sharekhan

El código siguiente muestra cómo inicializar [MiraeSharekhanMessageAdapter](xref:StockSharp.MiraeSharekhan.MiraeSharekhanMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MiraeSharekhanMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
	VendorKey = "<valor>",
	CustomerId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
