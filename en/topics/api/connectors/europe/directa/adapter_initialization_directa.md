# Adapter initialization: Directa

The following code initializes [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DirectaMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_directa.md) page.

## See also

[Connector configuration](configuration_directa.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
