# Zhongtai XTP アダプターの初期化

次のコードは、[XtpMessageAdapter](xref:StockSharp.Xtp.XtpMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new XtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	ClientId = 1,
	QuoteAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6001),
	TransactionAddress = new IPEndPoint(IPAddress.Parse("203.0.113.10"), 6002),
	Protocol = XtpProtocols.Tcp,
	SoftwareKey = "<ソフトウェアキー>",
	SoftwareVersion = "1.0",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)

