# Initialization of CoinAPI Adapter

The code below demonstrates how to initialize the [CoinApiMessageAdapter](xref:StockSharp.CoinApi.CoinApiMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CoinApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
