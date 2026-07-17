# Tiger Brokers adapter initialization

The following code demonstrates how to initialize [TigerBrokersMessageAdapter](xref:StockSharp.TigerBrokers.TigerBrokersMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TigerBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	TigerId = "<Tiger ID>",
	Account = "<Account>",
	License = TigerLicenses.Singapore,
	PrivateKey = "<Private key>".ToSecureString(),
	Token = "<Token>".ToSecureString(),
	AutoGrabPermission = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
