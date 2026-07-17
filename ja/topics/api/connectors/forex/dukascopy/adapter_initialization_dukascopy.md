# アダプターの初期化: Dukascopy JForex

次のコードは、[DukasCopyMessageAdapter](xref:StockSharp.DukasCopy.DukasCopyMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new DukasCopyMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<値>".ToSecureString(),
	UserName = "<値>",
	BridgeJarPath = "<値>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
