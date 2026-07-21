# Variational Omni アダプターの初期化

以下のコードは、[VariationalOmniMessageAdapter](xref:StockSharp.VariationalOmni.VariationalOmniMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new VariationalOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Endpoint = "<設定値>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
