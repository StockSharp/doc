# 适配器初始化：ESMA FIRDS

以下代码初始化 [EsmaFirdsMessageAdapter](xref:StockSharp.EsmaFirds.EsmaFirdsMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new EsmaFirdsMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_esma_firds.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_esma_firds.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
