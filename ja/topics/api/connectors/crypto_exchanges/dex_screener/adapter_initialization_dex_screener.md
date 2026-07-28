# アダプターの初期化: DEX Screener

次のコードは [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

[コネクタ設定](configuration_dex_screener.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_dex_screener.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
