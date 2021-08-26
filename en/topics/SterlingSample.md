# Adapter initialization Sterling

The code below demonstrates how to initialize the [SterlingMessageAdapter](../api/StockSharp.Sterling.SterlingMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new SterlingMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
