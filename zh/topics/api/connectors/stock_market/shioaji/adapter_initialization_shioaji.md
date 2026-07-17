# 适配器初始化: SinoPac Shioaji

以下代码演示如何初始化 [ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<值>".ToSecureString(),
	Secret = "<值>".ToSecureString(),
	Address = "<值>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
