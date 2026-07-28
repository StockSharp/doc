# アダプターの初期化: Velora

次のコードは [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<パートナー ID>",
	WalletAddress = "<ウォレットアドレス>",
	PrivateKey = "<秘密鍵>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_velora.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_velora.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
