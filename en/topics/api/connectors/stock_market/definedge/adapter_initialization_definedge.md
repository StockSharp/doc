# Adapter initialization: Definedge

The following code initializes [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_definedge.md) page.

## See also

[Connector configuration](configuration_definedge.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
