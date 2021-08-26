# Adapter initialization E\*TRADE

The code below demonstrates how to initialize the [ETradeMessageAdapter](../api/StockSharp.ETrade.ETradeMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new ETradeMessageAdapter(Connector.TransactionIdGenerator)
{
    ConsumerSecret \= "\<Your Secret\>".To\<SecureString\>(),
    ConsumerKey \= "\<Your Key\>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
