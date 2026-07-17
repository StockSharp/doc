# tastytrade 适配器初始化

以下代码演示如何初始化 [TastyTradeMessageAdapter](xref:StockSharp.TastyTrade.TastyTradeMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new TastyTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<令牌>".ToSecureString(),
	ClientSecret = "<客户端密钥>".ToSecureString(),
	Scopes = TastyTradeScopes.Read | TastyTradeScopes.Trade,
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将示例值替换为针对您的账户签发的凭据和服务器地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
