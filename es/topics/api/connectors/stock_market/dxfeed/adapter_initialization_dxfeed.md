# Inicialización del adaptador: dxFeed

El código siguiente muestra cómo inicializar [DxFeedMessageAdapter](xref:StockSharp.DxFeed.DxFeedMessageAdapter) y añadirlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DxFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<valor>".ToSecureString(),
	Address = "<valor>",
	MarketDepthSources = "<valor>",
	AggregationPeriod = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Sustituya los valores de ejemplo por los parámetros emitidos o configurados para su cuenta.

## Véase también

[Ventana de configuración de conexiones](../../../graphical_user_interface/connection_settings_window.md)
