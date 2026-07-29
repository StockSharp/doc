# アダプターの初期化: TraderMade

次のコードは [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<REST API キー>".To<SecureString>(),
	StreamingKey = "<ストリーミング API キー>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_tradermade.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_tradermade.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
