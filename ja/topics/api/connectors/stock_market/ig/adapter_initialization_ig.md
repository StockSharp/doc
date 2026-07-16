# IG Markets アダプターの初期化

次のコードは、[IgMessageAdapter](xref:StockSharp.IG.IgMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new IgMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<API キー>",
	UserName = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	AccountId = "<口座 ID>",
	Environment = IgEnvironments.Demo,
	EncryptPassword = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

