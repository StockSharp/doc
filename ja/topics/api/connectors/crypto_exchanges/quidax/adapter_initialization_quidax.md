# アダプターの初期化: Quidax

次のコードは [QuidaxMessageAdapter](xref:StockSharp.Quidax.QuidaxMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new QuidaxMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<アクセストークン>".To<SecureString>(),
	UserId = "<ユーザー ID>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_quidax.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_quidax.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
