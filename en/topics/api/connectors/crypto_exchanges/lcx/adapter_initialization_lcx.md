# Adapter initialization: LCX

The following code initializes [LcxMessageAdapter](xref:StockSharp.LCX.LcxMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new LcxMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_lcx.md) page.

## See also

[Connector configuration](configuration_lcx.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
