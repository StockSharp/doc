# アダプターの初期化: Capital Futures

次のコードは、[CapitalFuturesMessageAdapter](xref:StockSharp.CapitalFutures.CapitalFuturesMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new CapitalFuturesMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<値>".ToSecureString(),
	SdkPath = "<値>",
	Login = "<値>",
	Account = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
