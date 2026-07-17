# アダプターの初期化: Swissquote OpenWealth

次のコードは、[SwissquoteMessageAdapter](xref:StockSharp.Swissquote.SwissquoteMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new SwissquoteMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<値>".ToSecureString(),
	CustomerId = "<値>",
	SafekeepingAccountId = "<値>",
	CashAccountId = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
