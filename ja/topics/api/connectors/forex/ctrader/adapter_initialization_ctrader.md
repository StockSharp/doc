# cTrader アダプターの初期化

以下のコードは、[cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
