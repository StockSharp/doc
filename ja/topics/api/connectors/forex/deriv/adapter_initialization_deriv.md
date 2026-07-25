# アダプターの初期化: Deriv

次のコードは [DerivMessageAdapter](xref:StockSharp.Deriv.DerivMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
var messageAdapter = new DerivMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
	AppId = "<app-id>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

サンプル値を、選択したデモ口座または実口座に対して発行されたトークンとアプリケーション識別子に置き換えてください。

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
