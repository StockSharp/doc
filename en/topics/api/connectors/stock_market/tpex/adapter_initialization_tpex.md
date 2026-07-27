# Adapter initialization: TPEx

The following code initializes [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_tpex.md) page.

## See also

[Connector configuration](configuration_tpex.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
