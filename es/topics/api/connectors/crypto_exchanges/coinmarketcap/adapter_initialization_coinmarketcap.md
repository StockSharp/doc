# Inicialización del adaptador CoinMarketCap

El siguiente código muestra cómo inicializar [CoinMarketCapMessageAdapter](xref:StockSharp.CoinMarketCap.CoinMarketCapMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinMarketCapMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
	QuoteCurrency = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
