# Adapter initialization: AscendEX

The following code initializes [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_ascendex.md) page.

## See also

[Connector configuration](configuration_ascendex.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
