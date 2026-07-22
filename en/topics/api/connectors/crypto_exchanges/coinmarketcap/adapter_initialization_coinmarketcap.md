# Initialization of CoinMarketCap Adapter

The code below demonstrates how to initialize the [CoinMarketCapMessageAdapter](xref:StockSharp.CoinMarketCap.CoinMarketCapMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinMarketCapMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
	QuoteCurrency = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
