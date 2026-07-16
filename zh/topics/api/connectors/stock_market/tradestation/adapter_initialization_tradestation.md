# TradeStation 适配器初始化

以下代码演示如何初始化 [TradeStationMessageAdapter](xref:StockSharp.TradeStation.TradeStationMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TradeStationMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<令牌>".ToSecureString(),
	IsDemo = true,
	DefaultRoute = "Intelligent",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)

