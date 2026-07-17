# 适配器初始化: Bloomberg BLPAPI and EMSX

以下代码演示如何初始化 [BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<值>",
	EmsxService = "<值>",
	Broker = "<值>",
	ServerAddress = "<值>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
