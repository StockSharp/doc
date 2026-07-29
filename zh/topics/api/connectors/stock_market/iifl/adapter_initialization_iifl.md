# 适配器初始化：IIFL

以下代码初始化 [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API 机密>".To<SecureString>(),
	ClientId = "<您的客户端标识符>",
	AuthorizationCode = "<您的授权代码>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_iifl.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_iifl.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
