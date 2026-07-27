# アダプターの初期化: GLEIF

次のコードは [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_gleif.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_gleif.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
