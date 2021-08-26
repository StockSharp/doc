# Adapter initialization AlphaVantage

The code below demonstrates how to initialize the [AlphaVantageMessageAdapter](../api/StockSharp.AlphaVantage.AlphaVantageMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new AlphaVantageMessageAdapter(Connector.TransactionIdGenerator)
{
Token \= "\<Your Token\>".To\<SecureString\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
          
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
