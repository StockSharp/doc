# 适配器初始化: lemon.markets

以下代码演示如何初始化 [LemonMarketsMessageAdapter](xref:StockSharp.LemonMarkets.LemonMarketsMessageAdapter) 并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new LemonMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<值>".ToSecureString(),
	AccountId = "<值>",
	SecuritiesAccountId = "<值>",
	DataPrivacyPrincipal = "<值>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发或配置的参数。

## 另请参阅

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
