# アダプターの初期化: Ventura

次のコードは [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new VenturaMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	ClientId = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_ventura.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_ventura.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
