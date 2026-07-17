# 适配器初始化: Alice Blue

以下代码演示如何初始化 [AliceBlueMessageAdapter](xref:StockSharp.AliceBlue.AliceBlueMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new AliceBlueMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<值>".ToSecureString(),
	UserId = "<值>",
	ClientId = "<值>",
	DeviceId = "<值>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
