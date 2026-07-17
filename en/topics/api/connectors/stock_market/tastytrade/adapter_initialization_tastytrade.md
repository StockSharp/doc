# tastytrade adapter initialization

The following code demonstrates how to initialize [TastyTradeMessageAdapter](xref:StockSharp.TastyTrade.TastyTradeMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TastyTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	ClientSecret = "<Client secret>".ToSecureString(),
	Scopes = TastyTradeScopes.Read | TastyTradeScopes.Trade,
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
