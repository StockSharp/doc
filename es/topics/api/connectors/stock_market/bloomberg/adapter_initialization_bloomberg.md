# Inicialización del adaptador: Bloomberg BLPAPI and EMSX

El código siguiente muestra cómo inicializar [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<valor>",
	EmsxService = "<valor>",
	Broker = "<valor>",
	ServerAddress = "<valor>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
