# Adapter initialization: Groww

The following code shows how to initialize [GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<value>".ToSecureString(),
	ApiKey = "<value>".ToSecureString(),
	ApiSecret = "<value>".ToSecureString(),
	TotpSecret = "<value>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
