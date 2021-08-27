# Adapter initialization Yahoo

The code below demonstrates how to initialize the [YahooMessageAdapter](xref:StockSharp.Yahoo.YahooMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
