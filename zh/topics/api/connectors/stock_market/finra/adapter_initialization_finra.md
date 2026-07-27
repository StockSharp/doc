# 适配器初始化：FINRA

以下代码初始化 [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_finra.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_finra.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
