# アダプターの初期化: Zerodha Kite Connect

次のコードは、[ZerodhaMessageAdapter](xref:StockSharp.Zerodha.ZerodhaMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new ZerodhaMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiSecret = "<値>".ToSecureString(),
	Token = "<値>".ToSecureString(),
	RequestToken = "<値>".ToSecureString(),
	ApiKey = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
