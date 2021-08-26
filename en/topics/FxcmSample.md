# Adapter initialization FXCM

The code below demonstrates how to initialize the [FxcmMessageAdapter](../api/StockSharp.Fxcm.FxcmMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new FxcmMessageAdapter(Connector.TransactionIdGenerator)
{
    Login \= "\<Your Login\>",
    Password \= "\<Your Password\>".To\<SecureString\>(),
    Address \= "\<Your Address\>".To\<Uri\>(),
    IsDemo \= true
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
