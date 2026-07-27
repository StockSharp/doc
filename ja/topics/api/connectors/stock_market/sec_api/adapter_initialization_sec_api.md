# アダプターの初期化: SEC API

次のコードは [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_sec_api.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_sec_api.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
