# Adapter initialization MB Trading

The code below demonstrates how to initialize the [MBTradingMessageAdapter](../api/StockSharp.MBTrading.MBTradingMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
...	
var messageAdapter \= new MBTradingMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
};
...	
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
