# 适配器初始化: Shoonya

以下代码演示如何初始化 [ShoonyaMessageAdapter](xref:StockSharp.Shoonya.ShoonyaMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new ShoonyaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<值>".ToSecureString(),
	UserId = "<值>",
	AccountId = "<值>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
