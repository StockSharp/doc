# アダプターの初期化: SEC EDGAR

次のコードは [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SecEdgarMessageAdapter(connector.TransactionIdGenerator)
{
	UserAgent = "<アプリ名 your-email@example.com>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_sec_edgar.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_sec_edgar.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
