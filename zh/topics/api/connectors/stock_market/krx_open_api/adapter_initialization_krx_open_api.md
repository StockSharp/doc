# 适配器初始化：KRX Open API

以下代码初始化 [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_krx_open_api.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_krx_open_api.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
