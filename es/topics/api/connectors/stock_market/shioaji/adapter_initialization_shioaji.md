# Inicialización del adaptador: SinoPac Shioaji

El código siguiente muestra cómo inicializar [ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<valor>".ToSecureString(),
	Secret = "<valor>".ToSecureString(),
	Address = "<valor>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
