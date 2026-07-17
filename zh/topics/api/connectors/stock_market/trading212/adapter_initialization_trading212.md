# 适配器初始化: 交易 212

以下代码演示如何初始化 [Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<值>".ToSecureString(),
	ApiSecret = "<值>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
