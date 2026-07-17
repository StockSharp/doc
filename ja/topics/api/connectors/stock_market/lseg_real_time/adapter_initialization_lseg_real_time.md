# アダプターの初期化: LSEG Real-Time

次のコードは、[LsegRealTimeMessageAdapter](xref:StockSharp.LsegRealTime.LsegRealTimeMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new LsegRealTimeMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<値>".ToSecureString(),
	Secret = "<値>".ToSecureString(),
	Address = "<値>",
	StandbyAddress = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
