# Dow Jones アダプターの初期化

以下のコードは、[DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<設定値>".To<SecureString>(),
	ClientId = "<設定値>",
	Login = "<設定値>",
	Password = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
