# アダプターの初期化: Fubon Neo

次のコードは、[FubonNeoMessageAdapter](xref:StockSharp.FubonNeo.FubonNeoMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new FubonNeoMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<値>".ToSecureString(),
	ApiKey = "<値>".ToSecureString(),
	CertificatePassword = "<値>".ToSecureString(),
	SdkPath = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
