# Adapter initialization: Finage

The following code initializes [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Your API key>".To<SecureString>(),
	StreamingToken = "<Your streaming token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_finage.md) page.

## See also

[Connector configuration](configuration_finage.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
