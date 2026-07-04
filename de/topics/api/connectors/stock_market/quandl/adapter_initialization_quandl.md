# Adapter-Initialisierung Quandl

Der folgende Code zeigt, wie der [QuandlMessageAdapter](xref:StockSharp.Quandl.QuandlMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new QuandlMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
