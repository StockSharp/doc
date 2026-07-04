# Adapter-Initialisierung Tradier

Der folgende Code zeigt, wie der [TradierMessageAdapter](xref:StockSharp.Tradier.TradierMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
