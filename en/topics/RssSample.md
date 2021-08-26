# Adapter initialization RSS

The code below demonstrates how to initialize the [RssMessageAdapter](../api/StockSharp.Rss.RssMessageAdapter.html) and send it to [Connector](../api/StockSharp.Algo.Connector.html).

```cs
var messageAdapter \= new RssMessageAdapter(Connector.TransactionIdGenerator)
{
    Address \= new Uri("http:\/\/energy.rss"),
    CustomDateFormat \= "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](API_UI_ConnectorWindow.md)
