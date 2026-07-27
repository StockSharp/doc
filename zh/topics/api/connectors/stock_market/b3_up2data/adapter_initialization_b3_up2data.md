# 适配器初始化：B3 UP2DATA

以下代码初始化 [B3Up2DataMessageAdapter](xref:StockSharp.B3Up2Data.B3Up2DataMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new B3Up2DataMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_b3_up2data.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_b3_up2data.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
