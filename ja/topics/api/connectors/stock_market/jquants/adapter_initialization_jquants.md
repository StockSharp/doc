# アダプターの初期化: J-Quants

次のコードは [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new JQuantsMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_jquants.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_jquants.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
