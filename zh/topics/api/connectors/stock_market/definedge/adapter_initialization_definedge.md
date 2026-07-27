# 适配器初始化：Definedge

以下代码初始化 [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_definedge.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_definedge.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
