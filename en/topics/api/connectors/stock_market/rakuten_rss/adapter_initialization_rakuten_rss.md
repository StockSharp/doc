# Adapter initialization Rakuten MARKETSPEED II RSS

The code below demonstrates how to initialize the [RakutenRssMessageAdapter](xref:StockSharp.RakutenRss.RakutenRssMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RakutenRssMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
