# Adapter initialization RSS

The code below demonstrates how to initialize the [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
    Address = new Uri("http://energy.rss"),
    CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
