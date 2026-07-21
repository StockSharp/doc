# Initialization of GMX Adapter

The code below demonstrates how to initialize the [GmxMessageAdapter](xref:StockSharp.Gmx.GmxMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GmxMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
