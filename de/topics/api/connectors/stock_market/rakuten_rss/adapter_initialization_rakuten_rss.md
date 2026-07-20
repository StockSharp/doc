# Adapterinitialisierung Rakuten MARKETSPEED II RSS

Der folgende Code zeigt, wie der [RakutenRssMessageAdapter](xref:StockSharp.RakutenRss.RakutenRssMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RakutenRssMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
