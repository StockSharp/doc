# Inicialización del adaptador: Swissquote OpenWealth

El código siguiente muestra cómo inicializar [SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	CustomerId = "<valor>",
	SafekeepingAccountId = "<valor>",
	CashAccountId = "<valor>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
