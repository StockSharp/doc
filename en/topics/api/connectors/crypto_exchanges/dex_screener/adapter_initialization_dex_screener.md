# Adapter initialization: DEX Screener

The following code initializes [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_dex_screener.md) page.

## See also

[Connector configuration](configuration_dex_screener.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
