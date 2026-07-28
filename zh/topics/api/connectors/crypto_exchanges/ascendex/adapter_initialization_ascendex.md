# 适配器初始化：AscendEX

以下代码初始化 [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API Secret>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_ascendex.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_ascendex.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
