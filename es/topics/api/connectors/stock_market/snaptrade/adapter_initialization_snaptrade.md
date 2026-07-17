# Inicialización del adaptador: SnapTrade

El código siguiente muestra cómo inicializar [SnapTradeMessageAdapter](xref:StockSharp.SnapTrade.SnapTradeMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SnapTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<valor>".ToSecureString(),
	UserSecret = "<valor>".ToSecureString(),
	ClientId = "<valor>",
	UserId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
