# アダプターの初期化: Trading Economics

次のコードは [TradingEconomicsMessageAdapter](xref:StockSharp.TradingEconomics.TradingEconomicsMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new TradingEconomicsMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

[コネクタ設定](configuration_trading_economics.md)ページに記載された認証情報とその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_trading_economics.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
