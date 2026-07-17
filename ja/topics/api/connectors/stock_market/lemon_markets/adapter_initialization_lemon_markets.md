# アダプターの初期化: lemon.markets

次のコードは、[LemonMarketsMessageAdapter](xref:StockSharp.LemonMarkets.LemonMarketsMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new LemonMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<値>".ToSecureString(),
	AccountId = "<値>",
	SecuritiesAccountId = "<値>",
	DataPrivacyPrincipal = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
