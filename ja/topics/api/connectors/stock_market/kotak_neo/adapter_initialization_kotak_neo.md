# Kotak Neo アダプターの初期化

次のコードは、[KotakNeoMessageAdapter](xref:StockSharp.KotakNeo.KotakNeoMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new KotakNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	ConsumerKey = "<コンシューマーキー>".ToSecureString(),
	MobileNumber = "<携帯電話番号>",
	UserCode = "<ユーザーコード>",
	Mpin = "<MPIN>".ToSecureString(),
	TotpSecret = "<TOTP シークレット>".ToSecureString(),
	DefaultProduct = KotakNeoProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

