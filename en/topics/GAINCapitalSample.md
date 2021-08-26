# Adapter initialization GAIN Capital

The code below demonstrates how to initialize the [GainCapitalMessageAdapter](../api/StockSharp.GainCapital.GainCapitalMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new GainCapitalMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
