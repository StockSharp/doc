# Adapter initialization: WazirX

The following code initializes [WazirXMessageAdapter](xref:StockSharp.WazirX.WazirXMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new WazirXMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_wazirx.md) page.

## See also

[Connector configuration](configuration_wazirx.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
