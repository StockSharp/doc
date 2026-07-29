# Adapter initialization: IIFL

The following code initializes [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	ClientId = "<Your client identifier>",
	AuthorizationCode = "<Your authorization code>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_iifl.md) page.

## See also

[Connector configuration](configuration_iifl.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
