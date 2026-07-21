# Initialization of Kalshi Adapter

The code below demonstrates how to initialize the [KalshiMessageAdapter](xref:StockSharp.Kalshi.KalshiMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new KalshiMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
