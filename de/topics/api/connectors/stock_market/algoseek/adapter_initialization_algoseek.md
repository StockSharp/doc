# Adapterinitialisierung AlgoSeek

Der folgende Code zeigt, wie der [AlgoSeekMessageAdapter](xref:StockSharp.AlgoSeek.AlgoSeekMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AlgoSeekMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
