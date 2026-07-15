# Inicialización del adaptador TradeZero

El código siguiente muestra cómo inicializar [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<ruta de órdenes opcional>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Puede omitir `DefaultRoute` para que el conector seleccione automáticamente una ruta compatible.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
