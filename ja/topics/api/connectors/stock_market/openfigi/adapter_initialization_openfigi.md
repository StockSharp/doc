# アダプターの初期化: OpenFIGI

次のコードは [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new OpenFigiMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_openfigi.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_openfigi.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
