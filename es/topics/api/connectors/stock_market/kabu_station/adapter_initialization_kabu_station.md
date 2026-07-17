# Inicialización del adaptador: kabu Station

El código siguiente muestra cómo inicializar [KabuStationMessageAdapter](xref:StockSharp.KabuStation.KabuStationMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KabuStationMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiPassword = "<valor>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
