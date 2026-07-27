# Adapter initialization: InvertirOnline

The following code initializes [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new InvertirOnlineMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_invertironline.md) page.

## See also

[Connector configuration](configuration_invertironline.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
