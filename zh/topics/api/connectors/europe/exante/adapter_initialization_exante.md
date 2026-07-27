# 适配器初始化：EXANTE

以下代码初始化 [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new ExanteMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
	SummaryCurrency = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_exante.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_exante.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
