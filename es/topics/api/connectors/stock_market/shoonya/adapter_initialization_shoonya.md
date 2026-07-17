# Inicialización del adaptador: Shoonya

El código siguiente muestra cómo inicializar [ShoonyaMessageAdapter](xref:StockSharp.Shoonya.ShoonyaMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShoonyaMessageAdapter(Connector.TransactionIdGenerator)
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
