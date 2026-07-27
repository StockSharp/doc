# アダプターの初期化: Nubra

次のコードは [NubraMessageAdapter](xref:StockSharp.Nubra.NubraMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new NubraMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	DeviceId = "<id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_nubra.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_nubra.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
