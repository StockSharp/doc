# Adapter initialization Tradier

The code below demonstrates how to initialize the [TradierMessageAdapter](xref:StockSharp.Tradier.TradierMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradierMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
