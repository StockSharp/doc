# Gate.io アダプターの初期化

以下のコードは、[GateIOMessageAdapter](xref:StockSharp.GateIO.GateIOMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GateIOMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## 推奨コンテンツ

[?????????](../../../graphical_user_interface/connection_settings_window.md)

