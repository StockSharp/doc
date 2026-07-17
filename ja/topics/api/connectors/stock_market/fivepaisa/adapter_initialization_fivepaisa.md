# アダプターの初期化: 5paisa Xstream

次のコードは、[FivePaisaMessageAdapter](xref:StockSharp.FivePaisa.FivePaisaMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new FivePaisaMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<値>".ToSecureString(),
	AppKey = "<値>",
	ClientCode = "<値>",
	AlgoId = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
