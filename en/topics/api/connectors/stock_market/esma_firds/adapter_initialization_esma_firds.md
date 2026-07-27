# Adapter initialization: ESMA FIRDS

The following code initializes [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new EsmaFirdsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_esma_firds.md) page.

## See also

[Connector configuration](configuration_esma_firds.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
