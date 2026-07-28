# Adapter initialization: Buda

The following code initializes [BudaMessageAdapter](xref:StockSharp.Buda.BudaMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new BudaMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_buda.md) page.

## See also

[Connector configuration](configuration_buda.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
