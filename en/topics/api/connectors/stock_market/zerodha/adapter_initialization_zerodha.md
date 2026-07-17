# Adapter initialization: Zerodha Kite Connect

The following code shows how to initialize [ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<value>".ToSecureString(),
	Token = "<value>".ToSecureString(),
	RequestToken = "<value>".ToSecureString(),
	ApiKey = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
