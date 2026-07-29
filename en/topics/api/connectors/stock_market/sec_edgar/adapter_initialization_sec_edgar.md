# Adapter initialization: SEC EDGAR

The following code initializes [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<Your application your-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_sec_edgar.md) page.

## See also

[Connector configuration](configuration_sec_edgar.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
