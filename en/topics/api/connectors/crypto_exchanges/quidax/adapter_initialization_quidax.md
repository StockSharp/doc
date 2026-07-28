# Adapter initialization: Quidax

The following code initializes [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Your access token>".To<SecureString>(),
	UserId = "<Your user ID>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_quidax.md) page.

## See also

[Connector configuration](configuration_quidax.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
