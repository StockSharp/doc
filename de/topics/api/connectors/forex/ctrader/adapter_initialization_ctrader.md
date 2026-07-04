# cTrader-Adapter initialisieren

Der folgende Code zeigt, wie der [cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
