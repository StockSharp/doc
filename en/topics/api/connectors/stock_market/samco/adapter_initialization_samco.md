# Adapter initialization: Samco

The following code initializes [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SamcoMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_samco.md) page.

## See also

[Connector configuration](configuration_samco.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
