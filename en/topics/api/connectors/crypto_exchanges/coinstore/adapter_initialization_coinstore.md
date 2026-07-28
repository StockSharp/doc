# Adapter initialization: Coinstore

The following code initializes [CoinstoreMessageAdapter](xref:StockSharp.Coinstore.CoinstoreMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinstoreMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_coinstore.md) page.

## See also

[Connector configuration](configuration_coinstore.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
