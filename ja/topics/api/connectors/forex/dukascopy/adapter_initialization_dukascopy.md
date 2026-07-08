# DukasCopy アダプターの初期化

以下のコードは、[DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送る方法を示しています。

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator);
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...	
							
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
