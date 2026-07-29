# 适配器初始化：TraderMade

以下代码初始化 [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<您的 REST API 密钥>".To<SecureString>(),
	StreamingKey = "<您的流式 API 密钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_tradermade.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_tradermade.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
