# アダプターの初期化: Tradejini

次のコードは [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new TradejiniMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<key>".ToSecureString(),
	Password = "<secret>".ToSecureString(),
	TwoFactorCode = "<code>".ToSecureString(),
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_tradejini.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_tradejini.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
