# 适配器初始化：Coinstore

以下代码初始化 [CoinstoreMessageAdapter](xref:StockSharp.Coinstore.CoinstoreMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinstoreMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写凭据以及[连接器配置](configuration_coinstore.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_coinstore.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
