# アダプターの初期化: AscendEX

次のコードは [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_ascendex.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_ascendex.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
