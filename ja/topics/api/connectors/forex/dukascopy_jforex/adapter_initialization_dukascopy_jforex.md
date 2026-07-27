# アダプターの初期化: DukasCopy JForex

次のコードは、[DukasCopyJForexMessageAdapter](xref:StockSharp.DukasCopyJForex.DukasCopyJForexMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new DukasCopyJForexMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<値>".ToSecureString(),
	Login = "<値>",
	BridgeJarPath = "<値>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
