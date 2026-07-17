# Inicialización del adaptador: QMT

El código siguiente muestra cómo inicializar [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<valor>".ToSecureString(),
	GatewayHost = "<valor>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
