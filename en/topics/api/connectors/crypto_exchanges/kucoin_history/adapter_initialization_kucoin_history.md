# Adapter initialization Kucoin History

The code below demonstrates how to initialize the [KucoinHistoryMessageAdapter](xref:StockSharp.KucoinHistory.KucoinHistoryMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

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

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
