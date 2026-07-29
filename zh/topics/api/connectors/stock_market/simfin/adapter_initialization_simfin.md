# 适配器初始化：SimFin

以下代码初始化 [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_simfin.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_simfin.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
