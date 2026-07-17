# Inicialización del adaptador: Flattrade

El código siguiente muestra cómo inicializar [FlattradeMessageAdapter](xref:StockSharp.Flattrade.FlattradeMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FlattradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	UserId = "<valor>",
	AccountId = "<valor>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
