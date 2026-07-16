# Longbridge OpenAPI adapter initialization

The following code demonstrates how to initialize [LongbridgeMessageAdapter](xref:StockSharp.Longbridge.LongbridgeMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LongbridgeMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<Application key>",
	AppSecret = "<Application secret>".ToSecureString(),
	AccessToken = "<Access token>".ToSecureString(),
	Portfolio = "Longbridge",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

