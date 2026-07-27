# アダプターの初期化: Rupeezy

次のコードは [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new RupeezyMessageAdapter(Connector.TransactionIdGenerator)
{
	ApplicationId = "<id>",
	ApiKey = "<key>".ToSecureString(),
	AuthCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_rupeezy.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_rupeezy.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
