# Inicialización del adaptador: eToro

El código siguiente muestra cómo inicializar [EtoroMessageAdapter](xref:StockSharp.Etoro.EtoroMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EtoroMessageAdapter(Connector.TransactionIdGenerator)
{
	PublicApiKey = "<valor>".ToSecureString(),
	UserKey = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
