# Inicialización del adaptador: Motilal Oswal

El código siguiente muestra cómo inicializar [MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<valor>".ToSecureString(),
	Secret = "<valor>".ToSecureString(),
	Token = "<valor>".ToSecureString(),
	AccessToken = "<valor>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
