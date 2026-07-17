# アダプターの初期化: 取引テクノロジー

次のコードは、[TradingTechnologiesMessageAdapter](xref:StockSharp.TradingTechnologies.TradingTechnologiesMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new TradingTechnologiesMessageAdapter(Connector.TransactionIdGenerator)
{
	AppSecretKey = "<値>".ToSecureString(),
	SdkPath = "<値>",
	IsBinaryProtocol = true,
	IsOptionsEnabled = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
