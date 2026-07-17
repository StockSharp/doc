# NinjaTrader アダプターの初期化

次のコードは、[NinjaTraderMessageAdapter](xref:StockSharp.NinjaTrader.NinjaTraderMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new NinjaTraderMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	ClientId = "<クライアント ID>",
	Secret = "<シークレット>".ToSecureString(),
	AppId = "StockSharp",
	AppVersion = "1.0",
	DeviceId = Guid.NewGuid().ToString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
