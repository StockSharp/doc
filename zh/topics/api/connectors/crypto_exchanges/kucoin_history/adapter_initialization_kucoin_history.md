# Kucoin历史适配器初始化

下面的代码演示如何初始化 [KucoinHistoryMessageAdapter](xref:StockSharp.KucoinHistory.KucoinHistoryMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new();
...
var messageAdapter = new KucoinHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
