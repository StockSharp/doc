# アダプターの初期化: Tradernet

次のコードは [TradernetMessageAdapter](xref:StockSharp.Tradernet.TradernetMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new TradernetMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_tradernet.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_tradernet.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
