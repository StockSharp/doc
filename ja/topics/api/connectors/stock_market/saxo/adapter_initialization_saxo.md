# Saxo OpenAPI アダプターの初期化

次のコードは、[SaxoMessageAdapter](xref:StockSharp.Saxo.SaxoMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new SaxoMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<アクセストークン>".ToSecureString(),
	RefreshToken = "<更新トークン>".ToSecureString(),
	ClientId = "<クライアント ID>",
	ClientSecret = "<クライアントシークレット>".ToSecureString(),
	RedirectUri = "<リダイレクト URI>",
	AccountKey = "<口座キー>",
	Environment = SaxoEnvironments.Simulation,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
