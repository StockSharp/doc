# Rakuten MARKETSPEED II RSS 适配器初始化

下面的代码演示了如何初始化 [RakutenRssMessageAdapter](xref:StockSharp.RakutenRss.RakutenRssMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new RakutenRssMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
