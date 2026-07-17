# Adapter initialization: Bloomberg BLPAPI and EMSX

The following code shows how to initialize [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<value>",
	EmsxService = "<value>",
	Broker = "<value>",
	ServerAddress = "<value>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
