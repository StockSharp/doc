# 适配器初始化雅虎

下面的代码演示了如何初始化 [YahooMessageAdapter](xref:StockSharp.Yahoo.YahooMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new YahooMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
