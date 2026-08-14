# Adapter initialization CQG

The following code demonstrates how to initialize [CqgMessageAdapter](xref:StockSharp.CQG.CqgMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CqgMessageAdapter(Connector.TransactionIdGenerator)
{
	UserName = "<User name>",
	Password = "<Password>".ToSecureString(),
	PrivateLabel = "WebAPITest",
	ClientId = "WebAPITest",
	Endpoint = "wss://demoapi.cqg.com:443",
	Portfolio = "<Portfolio>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
