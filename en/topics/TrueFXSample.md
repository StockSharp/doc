# Adapter initialization TrueFX

The code below demonstrates how to initialize the [TrueFXMessageAdapter](../api/StockSharp.TrueFX.TrueFXMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
...	
var messageAdapter \= new TrueFXMessageAdapter(Connector.TransactionIdGenerator)
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
