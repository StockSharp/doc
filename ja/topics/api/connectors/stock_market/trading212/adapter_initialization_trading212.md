# アダプターの初期化: 取引 212

次のコードは、[Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<値>".ToSecureString(),
	ApiSecret = "<値>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
