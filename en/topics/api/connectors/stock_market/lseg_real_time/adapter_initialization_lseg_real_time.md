# Adapter initialization: LSEG Real-Time

The following code shows how to initialize [LsegRealTimeMessageAdapter](xref:StockSharp.LsegRealTime.LsegRealTimeMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new LsegRealTimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<value>".ToSecureString(),
	Secret = "<value>".ToSecureString(),
	Address = "<value>",
	StandbyAddress = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
