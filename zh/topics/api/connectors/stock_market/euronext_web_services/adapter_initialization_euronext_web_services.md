# 适配器初始化：Euronext Web Services

以下代码初始化 [EuronextWebServicesMessageAdapter](xref:StockSharp.EuronextWebServices.EuronextWebServicesMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new EuronextWebServicesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_euronext_web_services.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_euronext_web_services.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
