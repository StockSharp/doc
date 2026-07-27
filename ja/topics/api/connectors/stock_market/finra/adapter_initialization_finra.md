# アダプターの初期化: FINRA

次のコードは [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new FinraMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_finra.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_finra.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
