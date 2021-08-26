# Adapter initialization DukasCopy

The code below demonstrates how to initialize the [DukasCopyMessageAdapter](../api/StockSharp.DukasCopy.DukasCopyMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
