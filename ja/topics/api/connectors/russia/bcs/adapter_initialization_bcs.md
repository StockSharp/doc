# アダプターの初期化: BCS

次のコードは [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new BcsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_bcs.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_bcs.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
