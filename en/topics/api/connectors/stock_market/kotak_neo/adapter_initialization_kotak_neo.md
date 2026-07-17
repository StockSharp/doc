# Kotak Neo adapter initialization

The following code demonstrates how to initialize [KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<Consumer key>".ToSecureString(),
	MobileNumber = "<Mobile number>",
	UserCode = "<User code>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<TOTP secret>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
