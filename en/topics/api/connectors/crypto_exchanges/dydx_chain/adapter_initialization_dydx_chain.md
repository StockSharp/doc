# Initialization of dYdX Chain Adapter

The code below demonstrates how to initialize the [DydxChainMessageAdapter](xref:StockSharp.DydxChain.DydxChainMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DydxChainMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
