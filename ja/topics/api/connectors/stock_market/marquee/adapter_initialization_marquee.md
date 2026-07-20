# Goldman Sachs Marquee アダプターの初期化

以下のコードは、[MarqueeMessageAdapter](xref:StockSharp.Marquee.MarqueeMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に送信する方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new MarqueeMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<設定値>",
	ClientSecret = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
