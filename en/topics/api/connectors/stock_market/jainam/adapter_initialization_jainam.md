# Adapter initialization: Jainam

The following code initializes [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_jainam.md) page.

## See also

[Connector configuration](configuration_jainam.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
