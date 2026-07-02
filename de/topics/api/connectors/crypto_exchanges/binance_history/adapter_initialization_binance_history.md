# Adapterinitialisierung Binance History

Der folgende Code zeigt, wie der [BinanceHistoryMessageAdapter](xref:StockSharp.BinanceHistory.BinanceHistoryMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BinanceHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlener Inhalt

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
