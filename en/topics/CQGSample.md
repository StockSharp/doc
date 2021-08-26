# Adapter initialization CQG

The code below demonstrates how to initialize the [CqgComMessageAdapter](../api/StockSharp.Cqg.Com.CqgComMessageAdapter.html) and [CqgContinuumMessageAdapter](../api/StockSharp.Cqg.Continuum.CqgContinuumMessageAdapter.html) and and send it to [Connector](../api/StockSharp.Algo.Connector.html).

1. **CQG COM**, connection by local **CQG Integrated Client**:

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new CqgComMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

2. **CQG Continuum**, connecting directly to the server:

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new CqgContinuumMessageAdapter(Connector.TransactionIdGenerator)
{
    UserName \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    Address \= "\<Address\>".To\<IPAddress\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
