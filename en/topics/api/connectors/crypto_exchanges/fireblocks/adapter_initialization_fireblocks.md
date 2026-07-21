# Initialization of Fireblocks Adapter

The code below demonstrates how to initialize the [FireblocksMessageAdapter](xref:StockSharp.Fireblocks.FireblocksMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FireblocksMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
