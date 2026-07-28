# 适配器初始化：XRPL DEX

以下代码初始化 [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<您的 XRPL 账户地址>",
	Seed = "<您的机密家族种子>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写账户凭据以及[连接器配置](configuration_xrpl.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_xrpl.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
