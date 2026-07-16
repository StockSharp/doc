# Moomoo アダプターの初期化

次のコードは、[MoomooMessageAdapter](xref:StockSharp.Moomoo.MoomooMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new MoomooMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = new IPEndPoint(IPAddress.Loopback, 11111),
	Password = "<パスワード>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

