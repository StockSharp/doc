# Adapter initialization IQFeed

The code below demonstrates how to initialize the [IQFeedMessageAdapter](xref:StockSharp.IQFeed.IQFeedMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();				
...				
var messageAdapter = new IQFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Level1Address = "127.0.0.1:5009".To<EndPoint>(),
	Level2Address = "127.0.0.1:9200".To<EndPoint>(),
	LookupAddress = "127.0.0.1:9100".To<EndPoint>(),
	AdminAddress =  "127.0.0.1:9200".To<EndPoint>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
