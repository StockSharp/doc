# Adapter initialization: PPI

The following code initializes [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PpiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizedClient = "<id>",
	ClientKey = "<key>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_ppi.md) page.

## See also

[Connector configuration](configuration_ppi.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
