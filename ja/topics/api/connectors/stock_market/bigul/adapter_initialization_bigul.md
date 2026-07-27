# アダプターの初期化: Bigul

次のコードは [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new BigulMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientCode = "<id>",
	ApiKey = "<key>".ToSecureString(),
	ApiSecret = "<secret>".ToSecureString(),
	OneTimePassword = "<secret>".ToSecureString(),
	Token = "<token>".ToSecureString(),
	Source = "<id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_bigul.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_bigul.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
