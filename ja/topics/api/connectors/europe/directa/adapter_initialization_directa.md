# アダプターの初期化: Directa

次のコードは [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new DirectaMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_directa.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_directa.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
