# Adapterinitialisierung OptionMetrics IvyDB

Der folgende Code zeigt, wie der [OptionMetricsMessageAdapter](xref:StockSharp.OptionMetrics.OptionMetricsMessageAdapter) initialisiert und an den [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OptionMetricsMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
