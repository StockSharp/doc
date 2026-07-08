# Adapter-Initialisierung Yahoo

Der folgende Code zeigt, wie der [YahooMessageAdapter](xref:StockSharp.Yahoo.YahooMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
