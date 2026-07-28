# アダプターの初期化: DeepBook

次のコードは [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeepBookMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<ウォレットアドレス>",
	PrivateKey = "<秘密鍵>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_deepbook.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_deepbook.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
