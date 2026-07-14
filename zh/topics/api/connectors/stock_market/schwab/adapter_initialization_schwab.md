# Charles Schwab 适配器初始化

以下代码演示如何初始化 [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) 并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<访问令牌>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
