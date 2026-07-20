# Initialization of Jupiter Adapter

The code below demonstrates how to initialize the [JupiterMessageAdapter](xref:StockSharp.Jupiter.JupiterMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JupiterMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>".To<SecureString>(),
	WalletAddress = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
