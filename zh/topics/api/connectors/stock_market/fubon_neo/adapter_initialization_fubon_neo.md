# 适配器初始化: Fubon Neo

以下代码演示如何初始化 [FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<值>".ToSecureString(),
	ApiKey = "<值>".ToSecureString(),
	CertificatePassword = "<值>".ToSecureString(),
	SdkPath = "<值>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
