# Initialisierung des Gate.io-Verlaufsadapters

Der folgende Code zeigt, wie der [GateIOHistoryMessageAdapter](xref:StockSharp.GateIOHistory.GateIOHistoryMessageAdapter) initialisiert und zu [Connector](xref:StockSharp.Algo.Connector) hinzugefügt wird.

```cs
Connector connector = new();
...
var messageAdapter = new GateIOHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
