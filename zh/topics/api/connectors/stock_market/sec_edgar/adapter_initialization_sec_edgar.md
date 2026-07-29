# 适配器初始化：SEC EDGAR

以下代码初始化 [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<您的应用程序 your-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_sec_edgar.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_sec_edgar.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
