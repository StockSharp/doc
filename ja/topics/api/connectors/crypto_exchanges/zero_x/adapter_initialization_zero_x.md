# アダプターの初期化: 0x

次のコードは [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<API キー>".To<SecureString>(),
	WalletAddress = "<ウォレットアドレス>",
	PrivateKey = "<秘密鍵>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_zero_x.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_zero_x.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
