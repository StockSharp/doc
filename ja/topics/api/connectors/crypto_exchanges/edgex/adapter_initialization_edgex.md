# edgeX アダプターの初期化

以下のコードは、[EdgeXMessageAdapter](xref:StockSharp.EdgeX.EdgeXMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new EdgeXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<APIキー>".To<SecureString>(),
	Secret = "<APIシークレット>".To<SecureString>(),
	ClearingAccount = "<清算アカウント>",
	Passphrase = "<パスフレーズ>".To<SecureString>(),
	Section = EdgeXSections.Derivatives,
	EnableSpotSection = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
