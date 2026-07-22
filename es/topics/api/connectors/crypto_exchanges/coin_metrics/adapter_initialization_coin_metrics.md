# Inicialización del adaptador Coin Metrics

El siguiente código muestra cómo inicializar [CoinMetricsMessageAdapter](xref:StockSharp.CoinMetrics.CoinMetricsMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinMetricsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
