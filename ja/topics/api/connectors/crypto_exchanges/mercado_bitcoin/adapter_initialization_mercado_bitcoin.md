# Mercado Bitcoin アダプターの初期化

以下のコードは、[MercadoBitcoinMessageAdapter](xref:StockSharp.MercadoBitcoin.MercadoBitcoinMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MercadoBitcoinMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<設定値>".To<SecureString>(),
	Secret = "<設定値>".To<SecureString>(),
	AccountId = "<設定値>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
