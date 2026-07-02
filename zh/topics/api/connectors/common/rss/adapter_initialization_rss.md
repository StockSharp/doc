# RSS 适配器初始化

下面的代码演示了如何初始化 [RssMessageAdapter](xref:StockSharp.Rss.RssMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector) 中。

```cs
var messageAdapter = new RssMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new Uri("http://energy.rss"),
	CustomDateFormat = "ddd, dd MMM yyyy HH:mm:ss zzzz"
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
