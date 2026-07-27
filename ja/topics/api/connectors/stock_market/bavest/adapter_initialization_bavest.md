# アダプターの初期化: Bavest

次のコードは [BavestMessageAdapter](xref:StockSharp.Bavest.BavestMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new BavestMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_bavest.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_bavest.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
