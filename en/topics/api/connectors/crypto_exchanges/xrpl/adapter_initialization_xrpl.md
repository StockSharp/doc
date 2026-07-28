# Adapter initialization: XRPL DEX

The following code initializes [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<Your XRPL account>",
	Seed = "<Your family seed>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the account credentials and any other required properties described on the [Connector configuration](configuration_xrpl.md) page.

## See also

[Connector configuration](configuration_xrpl.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
