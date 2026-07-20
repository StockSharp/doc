# Adapter initialization OptionMetrics IvyDB

The code below demonstrates how to initialize the [OptionMetricsMessageAdapter](xref:StockSharp.OptionMetrics.OptionMetricsMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OptionMetricsMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
