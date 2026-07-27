# 适配器初始化：Ventura

以下代码初始化 [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_ventura.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_ventura.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
