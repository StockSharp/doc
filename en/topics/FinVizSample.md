# Adapter initialization DukasCopy

The code below demonstrates how to initialize the [FinVizMessageAdapter](../api/StockSharp.FinViz.FinVizMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new FinVizMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
