# 适配器初始化：Finam Trade API

以下代码初始化 [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter)，并将其添加到 [Connector](xref:StockSharp.Algo.Connector)。

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

请将 `Token` 设为 Finam Trade API 密钥。如果希望适配器使用令牌可访问的第一个账户，请不要设置 `AccountId`。其他属性请参阅[连接器配置](configuration_finam_trade.md)页面。

## 另请参阅

[连接器配置](configuration_finam_trade.md)

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
