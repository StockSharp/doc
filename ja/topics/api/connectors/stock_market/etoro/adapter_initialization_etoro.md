# アダプターの初期化: eToro

次のコードは、[EtoroMessageAdapter](xref:StockSharp.Etoro.EtoroMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new EtoroMessageAdapter(Connector.TransactionIdGenerator)
{
	PublicApiKey = "<値>".ToSecureString(),
	UserKey = "<値>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
