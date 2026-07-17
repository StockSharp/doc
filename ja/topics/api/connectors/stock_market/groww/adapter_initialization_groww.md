# アダプターの初期化: Groww

次のコードは、[GrowwMessageAdapter](xref:StockSharp.Groww.GrowwMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new GrowwMessageAdapter(Connector.TransactionIdGenerator)
{
	AccessToken = "<値>".ToSecureString(),
	ApiKey = "<値>".ToSecureString(),
	ApiSecret = "<値>".ToSecureString(),
	TotpSecret = "<値>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
