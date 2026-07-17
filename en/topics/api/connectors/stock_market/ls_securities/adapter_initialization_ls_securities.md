# Adapter initialization: LS Securities

The following code shows how to initialize [LsSecuritiesMessageAdapter](xref:StockSharp.LsSecurities.LsSecuritiesMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsSecuritiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppKey = "<value>".ToSecureString(),
	AppSecret = "<value>".ToSecureString(),
	Account = "<value>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
