# SBE 适配器初始化

以下代码初始化 [CryBroSBEMessageAdapter](xref:StockSharp.CryBro.SBE.CryBroSBEMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new CryBroSBEMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "127.0.0.1:5002".To<EndPoint>(),
	SenderCompId = "<login>",
	TargetCompId = "StockSharp",
	Password = "<password>".ToSecureString(),
	IsSupportNativeCandles = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

客户端和服务器必须使用兼容的 SBE 架构标识符和版本。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
