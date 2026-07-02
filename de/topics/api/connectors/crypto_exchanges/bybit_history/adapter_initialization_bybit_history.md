# Adapterinitialisierung Bybit History

Der folgende Code zeigt, wie der [BybitHistoryMessageAdapter](xref:StockSharp.BybitHistory.BybitHistoryMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) gesendet wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BybitHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlener Inhalt

[Fenster der Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
