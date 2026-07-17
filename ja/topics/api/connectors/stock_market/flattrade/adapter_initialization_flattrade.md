# アダプターの初期化: Flattrade

次のコードは、[FlattradeMessageAdapter](xref:StockSharp.Flattrade.FlattradeMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new FlattradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<値>".ToSecureString(),
	UserId = "<値>",
	AccountId = "<値>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
