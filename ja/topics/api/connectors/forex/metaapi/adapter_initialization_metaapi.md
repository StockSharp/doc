# アダプターの初期化: MetaApi

次のコードは [MetaApiMessageAdapter](xref:StockSharp.MetaApi.MetaApiMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new MetaApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

サンプル値を、MetaApi でデプロイした口座のトークンと識別子に置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
