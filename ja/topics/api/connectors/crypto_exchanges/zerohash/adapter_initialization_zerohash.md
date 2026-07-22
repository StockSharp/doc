# Zero Hash アダプターの初期化

以下のコードは、[ZeroHashMessageAdapter](xref:StockSharp.ZeroHash.ZeroHashMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ZeroHashMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<設定値>",
	Secret = "<設定値>".To<SecureString>(),
	Passphrase = "<設定値>".To<SecureString>(),
	Account = "<設定値>",
	User = "<設定値>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
