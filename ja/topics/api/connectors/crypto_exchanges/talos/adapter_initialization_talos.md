# Talos アダプターの初期化

以下のコードは、[TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<設定値>".To<EndPoint>(),
	SenderCompId = "<設定値>",
	TargetCompId = "<設定値>",
	Login = "<設定値>",
	Password = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
