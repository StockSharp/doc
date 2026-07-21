# Initialization of Orderly Network Adapter

The code below demonstrates how to initialize the [OrderlyNetworkMessageAdapter](xref:StockSharp.OrderlyNetwork.OrderlyNetworkMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OrderlyNetworkMessageAdapter(Connector.TransactionIdGenerator)
{
	AccountId = "<Your value>",
	Secret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
