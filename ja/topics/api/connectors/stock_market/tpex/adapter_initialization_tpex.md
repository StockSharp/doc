# アダプターの初期化: TPEx

次のコードは [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_tpex.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_tpex.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
