# アダプターの初期化: Databento

次のコードは、[DatabentoMessageAdapter](xref:StockSharp.Databento.DatabentoMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new DatabentoMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<値>".ToSecureString(),
	Dataset = "<値>",
	LiveAddress = "<値>",
	HistoricalAddress = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
