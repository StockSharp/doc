# アダプターの初期化: Settrade

次のコードは [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SettradeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
	AppCode = "<アプリケーションコード>",
	BrokerId = "<ブローカー識別子>",
	Account = "<口座番号>",
	Pin = "<取引 PIN>".To<SecureString>(),
	AccountType = SettradeAccountTypes.Equity,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

認証情報、口座種別、および[コネクタ設定](configuration_settrade.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_settrade.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
