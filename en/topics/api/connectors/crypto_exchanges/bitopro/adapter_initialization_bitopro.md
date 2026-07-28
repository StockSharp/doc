# Adapter initialization: BitoPro

The following code initializes [BitoProMessageAdapter](xref:StockSharp.BitoPro.BitoProMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BitoProMessageAdapter(connector.TransactionIdGenerator)
{
	Email = "<Your email>",
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_bitopro.md) page.

## See also

[Connector configuration](configuration_bitopro.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
