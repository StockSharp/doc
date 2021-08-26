# Adapter initialization Xignite

The code below demonstrates how to initialize the [XigniteMessageAdapter](../api/StockSharp.Xignite.XigniteMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter = new XigniteMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
