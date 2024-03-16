# Adapter initialization Binance History

The code below demonstrates how to initialize the [BinanceHistoryMessageAdapter](xref:StockSharp.BinanceHistory.BinanceHistoryMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new BinanceHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
