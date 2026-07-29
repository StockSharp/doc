# アダプターの初期化: IIFL

次のコードは [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new IIFLMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
	ClientId = "<クライアント識別子>",
	AuthorizationCode = "<認可コード>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_iifl.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_iifl.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
