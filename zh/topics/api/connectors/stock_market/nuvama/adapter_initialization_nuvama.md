# 适配器初始化：Nuvama

以下代码初始化 [NuvamaMessageAdapter](xref:StockSharp.Nuvama.NuvamaMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new NuvamaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestId = "<id>".ToSecureString(),
	AppIdKey = "<key>".ToSecureString(),
	PublicIpAddress = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_nuvama.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_nuvama.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
