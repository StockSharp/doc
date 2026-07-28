# アダプターの初期化: Pendle

次のコードは [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new PendleMessageAdapter(connector.TransactionIdGenerator)
{
	Chain = PendleChains.Ethereum,
	WalletAddress = "<ウォレットアドレス>",
	PrivateKey = "<秘密鍵>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

ウォレットの認証情報と、[コネクタ設定](configuration_pendle.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_pendle.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
