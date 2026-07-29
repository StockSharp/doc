# 适配器初始化：MarketData.app

以下代码初始化 [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new MarketDataAppMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<您的 API 令牌>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

请填写访问参数以及[连接器配置](configuration_marketdataapp.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_marketdataapp.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
