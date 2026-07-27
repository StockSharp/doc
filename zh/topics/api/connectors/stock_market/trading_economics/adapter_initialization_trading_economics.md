# 适配器初始化：Trading Economics

以下代码初始化 [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TradingEconomicsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请填写凭据以及[连接器配置](configuration_trading_economics.md)页面中说明的其他必需属性。

## 另请参阅

[连接器配置](configuration_trading_economics.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
