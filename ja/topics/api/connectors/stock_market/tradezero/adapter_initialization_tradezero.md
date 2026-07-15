# TradeZero アダプターの初期化

次のコードは、[TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<任意の注文ルート>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`DefaultRoute` を省略すると、コネクタが互換性のあるルートを選択します。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
