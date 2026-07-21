# Initialization of Synthetix Adapter

The code below demonstrates how to initialize the [SynthetixMessageAdapter](xref:StockSharp.Synthetix.SynthetixMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new SynthetixMessageAdapter(Connector.TransactionIdGenerator)
{
	SubAccountId = "<Your value>",
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
