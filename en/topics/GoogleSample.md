# Adapter initialization Google

The code below demonstrates how to initialize the [GoogleMessageAdapter](../api/StockSharp.Google.GoogleMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new GoogleMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
