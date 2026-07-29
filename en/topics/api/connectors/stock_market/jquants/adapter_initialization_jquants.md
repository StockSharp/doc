# Adapter initialization: J-Quants

The following code initializes [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new JQuantsMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_jquants.md) page.

## See also

[Connector configuration](configuration_jquants.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
