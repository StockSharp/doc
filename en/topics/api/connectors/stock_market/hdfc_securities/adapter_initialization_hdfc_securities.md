# Adapter initialization: HDFC Securities

The following code initializes [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new HdfcMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_hdfc_securities.md) page.

## See also

[Connector configuration](configuration_hdfc_securities.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
