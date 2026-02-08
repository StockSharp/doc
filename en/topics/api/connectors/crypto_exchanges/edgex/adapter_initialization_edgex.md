# Adapter initialization edgeX

The code below demonstrates how to initialize [EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
	ClearingAccount = "<Your Clearing Account>",
	Passphrase = "<Your Passphrase>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
