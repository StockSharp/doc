# Adapter initialization: TraderMade

The following code initializes [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<Your REST API key>".To<SecureString>(),
	StreamingKey = "<Your streaming API key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_tradermade.md) page.

## See also

[Connector configuration](configuration_tradermade.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
