# 适配器初始化：BitoPro

以下代码初始化 [BitoProMessageAdapter](xref:StockSharp.BitoPro.BitoProMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new BitoProMessageAdapter(connector.TransactionIdGenerator)
{
	Email = "<您的电子邮箱>",
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_bitopro.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_bitopro.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
