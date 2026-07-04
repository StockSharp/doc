# DukasCopy-Adapter initialisieren

Der folgende Code zeigt, wie der [DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
