# アダプターの初期化: Motilal Oswal

次のコードは、[MotilalOswalMessageAdapter](xref:StockSharp.MotilalOswal.MotilalOswalMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new MotilalOswalMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<値>".ToSecureString(),
	Secret = "<値>".ToSecureString(),
	Token = "<値>".ToSecureString(),
	AccessToken = "<値>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
