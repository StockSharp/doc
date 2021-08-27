# Adapter initialization DukasCopy

The code below demonstrates how to initialize the [FinVizMessageAdapter](xref:StockSharp.FinViz.FinVizMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FinVizMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
