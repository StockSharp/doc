# アダプターの初期化: XRPL DEX

次のコードは [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new XrplMessageAdapter(connector.TransactionIdGenerator)
{
	Account = "<XRPL アカウントアドレス>",
	Seed = "<秘密のファミリーシード>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アカウントの認証情報と、[コネクタ設定](configuration_xrpl.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_xrpl.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
