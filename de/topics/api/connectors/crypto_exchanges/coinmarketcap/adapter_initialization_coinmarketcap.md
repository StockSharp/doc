# Initialisierung des CoinMarketCap-Adapters

Der folgende Code zeigt, wie man den [CoinMarketCapMessageAdapter](xref:StockSharp.CoinMarketCap.CoinMarketCapMessageAdapter) initialisiert und ihn an den [Connector](xref:StockSharp.Algo.Connector) übergibt.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinMarketCapMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Ihr Wert>".To<SecureString>(),
	QuoteCurrency = "<Ihr Wert>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
