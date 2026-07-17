# 适配器初始化: QMT

以下代码演示如何初始化 [QmtMessageAdapter](xref:StockSharp.Qmt.QmtMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new QmtMessageAdapter(Connector.TransactionIdGenerator)
{
	GatewayToken = "<值>".ToSecureString(),
	GatewayHost = "<值>",
	GatewayPort = 10,
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
