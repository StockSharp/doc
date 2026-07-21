# Initialization of Aevo Adapter

The code below demonstrates how to initialize the [AevoMessageAdapter](xref:StockSharp.Aevo.AevoMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AevoMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	ApiSecret = "<Your value>".To<SecureString>(),
	WalletAddress = "<Your value>",
	SigningKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
