# アダプターの初期化: comdirect

次のコードは [ComdirectMessageAdapter](xref:StockSharp.Comdirect.ComdirectMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new ComdirectMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_comdirect.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_comdirect.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
