# Tradovate 适配器初始化

以下代码演示如何初始化 [TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<用户名>",
	Password = "<密码>".ToSecureString(),
	ClientId = "<API 客户端标识符>",
	Secret = "<API 客户端密钥>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<固定设备标识符>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

将 `IsDemo` 设为 `false` 可连接到实盘环境。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
