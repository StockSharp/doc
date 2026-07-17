# Angel One アダプターの初期化

次のコードは、[AngelOneMessageAdapter](xref:StockSharp.AngelOne.AngelOneMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new AngelOneMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	ApiKey = "<API キー>".ToSecureString(),
	TotpSecret = "<TOTP シークレット>".ToSecureString(),
	ClientLocalIp = "127.0.0.1",
	ClientPublicIp = "<クライアントの公開 IP アドレス>",
	MacAddress = "<MAC アドレス>",
	DefaultProduct = AngelOneProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
