# Initialisierung des Kucoin-Verlaufsadapters

Der folgende Code zeigt, wie der [KucoinHistoryMessageAdapter](xref:StockSharp.KucoinHistory.KucoinHistoryMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
Connector connector = new();
...
var messageAdapter = new KucoinHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
