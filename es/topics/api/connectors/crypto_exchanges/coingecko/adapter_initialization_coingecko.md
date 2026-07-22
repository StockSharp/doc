# Inicialización del adaptador CoinGecko

El siguiente código muestra cómo inicializar [CoinGeckoMessageAdapter](xref:StockSharp.CoinGecko.CoinGeckoMessageAdapter) y pasarlo a [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinGeckoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Su valor>".To<SecureString>(),
	QuoteCurrency = "<Su valor>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Contenido recomendado

[Ventana de configuración de conexión](../../../graphical_user_interface/connection_settings_window.md)
