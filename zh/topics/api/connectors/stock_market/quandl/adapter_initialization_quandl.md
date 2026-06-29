# 适配器初始化 Quandl

下面的代码演示了如何初始化 [QuandlMessageAdapter](xref:StockSharp.Quandl.QuandlMessageAdapter) 并将其发送到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new QuandlMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
