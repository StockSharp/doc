# Copper アダプターの初期化

以下のコードは、[CopperMessageAdapter](xref:StockSharp.Copper.CopperMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CopperMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<設定値>",
	ApiSecret = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
