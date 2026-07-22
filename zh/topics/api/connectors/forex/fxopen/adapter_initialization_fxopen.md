# 适配器初始化：FXOpen TickTrader

以下代码初始化 [FXOpenMessageAdapter](xref:StockSharp.FXOpen.FXOpenMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new FXOpenMessageAdapter(Connector.TransactionIdGenerator)
{
	WebApiId = "<web-api-id>",
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为所选实盘或模拟账户的令牌参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
