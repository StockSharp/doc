# Inicialización del adaptador TradeStation

El código siguiente muestra cómo inicializar [TradeStationMessageAdapter](xref:StockSharp.TradeStation.TradeStationMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeStationMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	IsDemo = true,
	DefaultRoute = "Intelligent",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por las credenciales y direcciones de servidores emitidas para su cuenta.

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
