# アダプターの初期化: KRX Open API

次のコードは [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new KrxOpenApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_krx_open_api.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_krx_open_api.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
