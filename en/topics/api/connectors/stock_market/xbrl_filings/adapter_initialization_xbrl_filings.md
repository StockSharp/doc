# Adapter initialization: XBRL Filings

The following code initializes [XbrlFilingsMessageAdapter](xref:StockSharp.XbrlFilings.XbrlFilingsMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new XbrlFilingsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_xbrl_filings.md) page.

## See also

[Connector configuration](configuration_xbrl_filings.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
