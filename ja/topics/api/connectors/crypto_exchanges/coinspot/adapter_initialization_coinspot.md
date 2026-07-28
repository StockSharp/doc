# アダプターの初期化: CoinSpot

次のコードは [CoinSpotMessageAdapter](xref:StockSharp.CoinSpot.CoinSpotMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinSpotMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_coinspot.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_coinspot.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
