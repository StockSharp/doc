# Kucoin履歴アダプターの初期化

以下のコードは、[KucoinHistoryMessageAdapter](xref:StockSharp.KucoinHistory.KucoinHistoryMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加する方法を示しています。

```cs
Connector connector = new();
...
var messageAdapter = new KucoinHistoryMessageAdapter(connector.TransactionIdGenerator)
{
	CheckDates = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
