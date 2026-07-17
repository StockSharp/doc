# Inicialización del adaptador: Negociación 212

El código siguiente muestra cómo inicializar [Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<valor>".ToSecureString(),
	ApiSecret = "<valor>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
