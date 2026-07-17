# Adapter initialization: Trading Technologies

The following code shows how to initialize [TradingTechnologiesMessageAdapter](xref:StockSharp.TradingTechnologies.TradingTechnologiesMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradingTechnologiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppSecretKey = "<value>".ToSecureString(),
	SdkPath = "<value>",
	IsBinaryProtocol = true,
	IsOptionsEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
