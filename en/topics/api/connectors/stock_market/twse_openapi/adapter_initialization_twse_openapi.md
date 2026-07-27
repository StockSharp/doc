# Adapter initialization: TWSE

The following code initializes [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_twse_openapi.md) page.

## See also

[Connector configuration](configuration_twse_openapi.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
