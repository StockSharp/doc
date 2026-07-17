# アダプターの初期化: OpenMarkets

次のコードは、[OpenMarketsMessageAdapter](xref:StockSharp.OpenMarkets.OpenMarketsMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に追加する方法を示します。

```cs
var messageAdapter = new OpenMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientSecret = "<値>".ToSecureString(),
	ClientId = "<値>",
	AccountCode = "<値>",
	DataSource = "<値>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

例の値を、口座用に発行または設定されたパラメーターに置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
