# アダプターの初期化: Chainflip

次のコードは [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<EVM ウォレットアドレス>",
	PrivateKey = "<EVM 秘密鍵>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

ウォレットの認証情報、宛先アドレス、および[コネクタ設定](configuration_chainflip.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_chainflip.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
