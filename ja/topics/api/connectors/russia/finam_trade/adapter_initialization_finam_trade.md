# アダプターの初期化: Finam Trade API

次のコードは [FinamTradeMessageAdapter](xref:StockSharp.FinamTrade.FinamTradeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new FinamTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AccountId = "<account-id>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`Token` に Finam Trade API シークレットを設定します。アダプターにトークンで利用できる最初の口座を使わせる場合は、`AccountId` を省略します。追加のプロパティは[コネクタ設定](configuration_finam_trade.md)ページに記載されています。

## 関連項目

[コネクタ設定](configuration_finam_trade.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
