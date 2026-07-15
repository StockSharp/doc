# Tradovate アダプターの初期化

次のコードは、[TradovateMessageAdapter](xref:StockSharp.Tradovate.TradovateMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new TradovateMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	ClientId = "<API クライアント ID>",
	Secret = "<API クライアントのシークレット>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = "<固定のデバイス ID>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

実環境に接続するには、`IsDemo` を `false` に設定します。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
