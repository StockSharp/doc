# アダプターの初期化: Jainam

次のコードは [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new JainamMessageAdapter(Connector.TransactionIdGenerator)
{
	UserId = "<id>",
	AppCode = "<id>",
	ApiSecret = "<secret>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_jainam.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_jainam.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
