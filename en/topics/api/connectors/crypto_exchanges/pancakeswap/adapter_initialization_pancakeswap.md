# Initialization of PancakeSwap Adapter

The code below demonstrates how to initialize the [PancakeSwapMessageAdapter](xref:StockSharp.PancakeSwap.PancakeSwapMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PancakeSwapMessageAdapter(Connector.TransactionIdGenerator)
{
	GraphApiKey = "<Your value>".To<SecureString>(),
	WalletAddress = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
