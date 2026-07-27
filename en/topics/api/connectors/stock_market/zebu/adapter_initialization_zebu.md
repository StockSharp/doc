# Adapter initialization: Zebu

The following code initializes [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new ZebuMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	UserId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_zebu.md) page.

## See also

[Connector configuration](configuration_zebu.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
