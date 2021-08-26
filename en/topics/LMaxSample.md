# Adapter initialization LMAX

The code below demonstrates how to initialize the [LmaxMessageAdapter](../api/StockSharp.LMAX.LmaxMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new LmaxMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    IsDemo \= true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
