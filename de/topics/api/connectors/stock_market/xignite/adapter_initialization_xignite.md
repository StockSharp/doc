# Adapter-Initialisierung Xignite

Der folgende Code zeigt, wie der [XigniteMessageAdapter](xref:StockSharp.Xignite.XigniteMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
