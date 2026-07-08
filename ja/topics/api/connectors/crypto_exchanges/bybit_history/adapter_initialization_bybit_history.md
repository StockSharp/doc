# アダプターの初期化 Bybit History

以下のコードは、[BybitHistoryMessageAdapter](xref:StockSharp.BybitHistory.BybitHistoryMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BybitHistoryMessageAdapter(Connector.TransactionIdGenerator)
{
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

## 推奨コンテンツ

[?????????](../../../graphical_user_interface/connection_settings_window.md)
