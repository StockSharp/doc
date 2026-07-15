# TradeZero 适配器初始化

以下代码演示如何初始化 [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<可选订单路由>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

可以省略 `DefaultRoute`，让连接器选择兼容的路由。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
