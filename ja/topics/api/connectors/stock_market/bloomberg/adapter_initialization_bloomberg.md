# アダプターの初期化: Bloomberg BLPAPI and EMSX

次のコードは、[BloombergMessageAdapter](xref:StockSharp.Bloomberg.BloombergMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new BloombergMessageAdapter(Connector.TransactionIdGenerator)
{
	SdkPath = "<値>",
	EmsxService = "<値>",
	Broker = "<値>",
	ServerAddress = "<値>".To<EndPoint>(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
