# Initialization of CoinGecko Adapter

The code below demonstrates how to initialize the [CoinGeckoMessageAdapter](xref:StockSharp.CoinGecko.CoinGeckoMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinGeckoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
	QuoteCurrency = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
