# アダプターの初期化: SinoPac Shioaji

次のコードは、[ShioajiMessageAdapter](xref:StockSharp.Shioaji.ShioajiMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new ShioajiMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<値>".ToSecureString(),
	Secret = "<値>".ToSecureString(),
	Address = "<値>",
	ReconnectAttempts = 10,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
