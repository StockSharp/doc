# 适配器初始化 OKEx 历史

下面的代码演示了如何初始化 [OkexHistoryMessageAdapter](xref:StockSharp.OkexHistory.OkexHistoryMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new OkexHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
