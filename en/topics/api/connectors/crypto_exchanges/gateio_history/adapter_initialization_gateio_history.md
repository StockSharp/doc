# Adapter initialization Gate.io History

The code below demonstrates how to initialize the [GateIOHistoryMessageAdapter](xref:StockSharp.GateIOHistory.GateIOHistoryMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

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

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
