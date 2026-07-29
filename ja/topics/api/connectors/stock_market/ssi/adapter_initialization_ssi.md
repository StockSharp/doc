# アダプターの初期化: SSI

次のコードは [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new SSIMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
	ClientId = "<クライアント識別子>",
	PrivateKey = "<RSA 秘密鍵>".To<SecureString>(),
	Otp = "<現在の OTP>".To<SecureString>(),
	Account = "<口座番号>",
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

アクセス情報と、[コネクタ設定](configuration_ssi.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_ssi.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
