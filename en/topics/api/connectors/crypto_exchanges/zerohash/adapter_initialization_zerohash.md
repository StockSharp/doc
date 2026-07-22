# Initialization of Zero Hash Adapter

The code below demonstrates how to initialize the [ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	Secret = "<Your value>".To<SecureString>(),
	Passphrase = "<Your value>".To<SecureString>(),
	Account = "<Your value>",
	User = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
