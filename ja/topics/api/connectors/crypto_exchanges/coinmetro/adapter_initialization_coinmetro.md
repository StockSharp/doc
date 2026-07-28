# アダプターの初期化: Coinmetro

次のコードは [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<アクセストークン>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_coinmetro.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_coinmetro.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
