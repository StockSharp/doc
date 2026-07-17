# Adapter initialization: Fubon Neo

The following code shows how to initialize [FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<value>".ToSecureString(),
	ApiKey = "<value>".ToSecureString(),
	CertificatePassword = "<value>".ToSecureString(),
	SdkPath = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
