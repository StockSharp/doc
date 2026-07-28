# アダプターの初期化: STON.fi

次のコードは [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に追加します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<TON ウォレットアドレス>",
	Mnemonic = "<24 語のニーモニックフレーズ>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

ウォレットの認証情報と、[コネクタ設定](configuration_stonfi.md)ページに記載されたその他の必須プロパティを設定してください。

## 関連項目

[コネクタ設定](configuration_stonfi.md)

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
