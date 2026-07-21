# Initialization of Reya Adapter

The code below demonstrates how to initialize the [ReyaMessageAdapter](xref:StockSharp.Reya.ReyaMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ReyaMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your value>",
	AccountId = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
