# 适配器初始化: 交易技术

以下代码演示如何初始化 [TradingTechnologiesMessageAdapter](xref:StockSharp.TradingTechnologies.TradingTechnologiesMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TradingTechnologiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppSecretKey = "<值>".ToSecureString(),
	SdkPath = "<值>",
	IsBinaryProtocol = true,
	IsOptionsEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
