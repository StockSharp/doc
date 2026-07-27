# アダプターの初期化: MasterLink

次のコードは [MasterLinkMessageAdapter](xref:StockSharp.MasterLink.MasterLinkMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new MasterLinkMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	CertificatePath = "<id>",
	CertificatePassword = "<secret>".ToSecureString(),
	NodePath = "<id>",
	GatewayDirectory = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_masterlink.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_masterlink.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
