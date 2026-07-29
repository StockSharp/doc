# アダプターの初期化: Finage

次のコードは [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<API キー>".To<SecureString>(),
	StreamingToken = "<ストリーミングトークン>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_finage.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_finage.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
