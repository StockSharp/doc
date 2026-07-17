# Adapter initialization: Capital Futures

The following code shows how to initialize [CapitalFuturesMessageAdapter](xref:StockSharp.CapitalFutures.CapitalFuturesMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new CapitalFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<value>".ToSecureString(),
	SdkPath = "<value>",
	Login = "<value>",
	Account = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
