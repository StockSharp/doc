# Adapter initialization: NovaDAX

The following code initializes [NovaDaxMessageAdapter](xref:StockSharp.NovaDax.NovaDaxMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new NovaDaxMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	AccountId = "<Your account ID>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_novadax.md) page.

## See also

[Connector configuration](configuration_novadax.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
