# Adapter initialization Interactive Brokers

The code below demonstrates how to initialize the [InteractiveBrokersMessageAdapter](../api/StockSharp.InteractiveBrokers.InteractiveBrokersMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
Connector Connector \= new Connector();				
...				
var messageAdapter \= new InteractiveBrokersMessageAdapter(Connector.TransactionIdGenerator)
{
	Address \= "\<Your Address\>".To\<EndPoint\>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
