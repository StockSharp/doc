# ICICI Direct Breeze adapter initialization

The following code demonstrates how to initialize [BreezeMessageAdapter](xref:StockSharp.Breeze.BreezeMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BreezeMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API key>",
	SecretKey = "<Secret key>".ToSecureString(),
	ApiSession = "<API session>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
