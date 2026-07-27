# アダプターの初期化: Mastertrust

次のコードは [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new MastertrustMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<id>",
	OAuthClientSecret = "<secret>".ToSecureString(),
	AuthorizationCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_mastertrust.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_mastertrust.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
