# Adapter initialization: GLEIF

The following code initializes [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_gleif.md) page.

## See also

[Connector configuration](configuration_gleif.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
