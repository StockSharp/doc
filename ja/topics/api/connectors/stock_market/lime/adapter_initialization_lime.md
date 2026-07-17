# Lime Trader アダプターの初期化

次のコードは、[LimeMessageAdapter](xref:StockSharp.Lime.LimeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new LimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	ClientId = "<クライアント ID>",
	ClientSecret = "<クライアントシークレット>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
