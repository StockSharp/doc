# Angel One adapter initialization

The following code demonstrates how to initialize [AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Login>",
	Password = "<Password>".ToSecureString(),
	ApiKey = "<API key>".ToSecureString(),
	TotpSecret = "<TOTP secret>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<Client public IP address>",
	MacAddress = "<MAC address>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
