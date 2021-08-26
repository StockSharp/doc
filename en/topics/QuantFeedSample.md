# Adapter initialization QuantFEED

The code below demonstrates how to initialize the [QuantFeedMessageAdapter](../api/StockSharp.QuantHouse.QuantFeedMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new QuantFeedMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    Address \= "\<Address\>".To\<EndPoint\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
