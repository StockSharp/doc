# Adapter initialization: SimFin

The following code initializes [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the access values and any other required properties described on the [Connector configuration](configuration_simfin.md) page.

## See also

[Connector configuration](configuration_simfin.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
