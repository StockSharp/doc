# 适配器初始化：Primary

以下代码初始化 [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_primary.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_primary.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
