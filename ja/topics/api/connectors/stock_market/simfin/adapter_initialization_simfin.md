# アダプターの初期化: SimFin

次のコードは [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SimFinMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_simfin.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_simfin.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
