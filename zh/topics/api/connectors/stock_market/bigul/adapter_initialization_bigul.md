# 适配器初始化：Bigul

以下代码初始化 [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_bigul.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_bigul.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
