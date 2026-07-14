# Webull アダプターの初期化

以下のコードは、[WebullMessageAdapter](xref:StockSharp.Webull.WebullMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
var messageAdapter = new WebullMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<アプリケーションキー>".ToSecureString(),
	Secret = "<アプリケーションシークレット>".ToSecureString(),
	Token = "<アクセストークン>".ToSecureString(),
	Account = "<口座識別子>",
	IsDemo = false,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`Token` と `Account` は不要な場合に省略できます。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
