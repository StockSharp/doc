# 适配器初始化: Motilal Oswal

以下代码演示如何初始化 [MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<值>".ToSecureString(),
	Secret = "<值>".ToSecureString(),
	Token = "<值>".ToSecureString(),
	AccessToken = "<值>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
