# Inicialización del adaptador: Zerodha Kite Connect

El código siguiente muestra cómo inicializar [ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<valor>".ToSecureString(),
	Token = "<valor>".ToSecureString(),
	RequestToken = "<valor>".ToSecureString(),
	ApiKey = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
