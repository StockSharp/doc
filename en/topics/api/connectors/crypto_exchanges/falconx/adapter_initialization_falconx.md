# Initialization of FalconX Adapter

The code below demonstrates how to initialize the [FalconXMessageAdapter](xref:StockSharp.FalconX.FalconXMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FalconXMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	Secret = "<Your value>".To<SecureString>(),
	Passphrase = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
