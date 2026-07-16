# IG Markets adapter initialization

The following code demonstrates how to initialize [IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API key>",
	UserName = "<User name>",
	Password = "<Password>".ToSecureString(),
	AccountId = "<Account ID>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

