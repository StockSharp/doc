# アダプターの初期化: Primary

次のコードは [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new PrimaryMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<id>",
	Password = "<secret>".ToSecureString(),
	IsDemo = true,
	Account = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_primary.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_primary.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
