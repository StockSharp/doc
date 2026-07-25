# 适配器初始化：Deriv

以下代码初始化 [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为所选模拟或实盘账户所签发的令牌和应用程序标识符。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
