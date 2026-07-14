# Charles Schwab アダプターの初期化

以下のコードは、[SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<アクセストークン>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
