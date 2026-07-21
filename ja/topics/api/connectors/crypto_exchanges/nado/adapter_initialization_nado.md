# Nado アダプターの初期化

以下のコードは、[NadoMessageAdapter](xref:StockSharp.Nado.NadoMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new NadoMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<設定値>",
	PrivateKey = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
