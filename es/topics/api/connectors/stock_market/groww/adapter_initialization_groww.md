# Inicialización del adaptador: Groww

El código siguiente muestra cómo inicializar [GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<valor>".ToSecureString(),
	ApiKey = "<valor>".ToSecureString(),
	ApiSecret = "<valor>".ToSecureString(),
	TotpSecret = "<valor>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
