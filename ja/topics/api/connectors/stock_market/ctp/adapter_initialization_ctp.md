# CTP アダプターの初期化

次のコードは、[CtpMessageAdapter](xref:StockSharp.Ctp.CtpMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
var messageAdapter = new CtpMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<ユーザー名>",
	Password = "<パスワード>".ToSecureString(),
	BrokerId = "<ブローカー ID>",
	InvestorId = "<投資家 ID>",
	MarketDataAddress = "tcp://<市場データアドレス>",
	TraderAddress = "tcp://<取引サーバーアドレス>",
	AppId = "<アプリケーション ID>",
	AuthCode = "<認証コード>".ToSecureString(),
	ProductionMode = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行された認証情報とサーバーアドレスに置き換えてください。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
