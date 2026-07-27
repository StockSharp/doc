# アダプターの初期化: TWSE

次のコードは [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_twse_openapi.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_twse_openapi.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
