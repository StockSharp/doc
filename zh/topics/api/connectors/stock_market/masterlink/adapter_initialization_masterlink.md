# 适配器初始化：MasterLink

以下代码初始化 [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_masterlink.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_masterlink.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
