# アダプターの初期化: m.Stock

次のコードは [MStockMessageAdapter](xref:StockSharp.MStock.MStockMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new MStockMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	ClientCode = "<クライアントコード>",
	Password = "<パスワード>".To<SecureString>(),
	Otp = "<現在の OTP>".To<SecureString>(),
	UseTotp = true,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_mstock.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_mstock.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
