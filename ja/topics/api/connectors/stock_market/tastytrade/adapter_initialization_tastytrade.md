# tastytrade アダプターの初期化

次のコードは、[TastyTradeMessageAdapter](xref:StockSharp.TastyTrade.TastyTradeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new TastyTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<トークン>".ToSecureString(),
	ClientSecret = "<クライアントシークレット>".ToSecureString(),
	Scopes = TastyTradeScopes.Read | TastyTradeScopes.Trade,
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

