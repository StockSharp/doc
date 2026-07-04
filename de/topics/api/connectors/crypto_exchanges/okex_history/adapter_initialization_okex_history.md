# OKEx-History-Adapter initialisieren

Der folgende Code zeigt, wie der [OkexHistoryMessageAdapter](xref:StockSharp.OkexHistory.OkexHistoryMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OkexHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)

