# TradeLocker アダプターの初期化

以下のコードは、[TradeLockerMessageAdapter](xref:StockSharp.TradeLocker.TradeLockerMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TradeLockerMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<設定値>",
	Password = "<設定値>".To<SecureString>(),
	AccountId = "<設定値>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
