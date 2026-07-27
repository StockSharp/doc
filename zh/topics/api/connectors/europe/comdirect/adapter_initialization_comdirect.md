# 适配器初始化：comdirect

以下代码初始化 [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_comdirect.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_comdirect.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
