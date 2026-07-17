# Adapter initialization: Databento

The following code shows how to initialize [DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<value>".ToSecureString(),
	Dataset = "<value>",
	LiveAddress = "<value>",
	HistoricalAddress = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
