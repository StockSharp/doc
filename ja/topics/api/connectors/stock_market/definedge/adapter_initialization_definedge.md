# アダプターの初期化: Definedge

次のコードは [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new DefinedgeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	WebSocketToken = "<token>".ToSecureString(),
	UserId = "<id>",
	AccountId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_definedge.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_definedge.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
