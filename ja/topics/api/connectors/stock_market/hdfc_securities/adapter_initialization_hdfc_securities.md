# アダプターの初期化: HDFC Securities

次のコードは [HdfcMessageAdapter](xref:StockSharp.HdfcSecurities.HdfcMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new HdfcMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<key>".ToSecureString(),
	Secret = "<secret>".ToSecureString(),
	RequestToken = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_hdfc_securities.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_hdfc_securities.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
