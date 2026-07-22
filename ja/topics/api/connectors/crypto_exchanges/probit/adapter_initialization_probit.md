# ProBit Global アダプターの初期化

次のコードは、[ProBitMessageAdapter](xref:StockSharp.ProBit.ProBitMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new ProBitMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<OAuth クライアント ID>".To<SecureString>(),
	Secret = "<OAuth クライアントシークレット>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

公開市場データだけを利用する場合は、`Key` と `Secret` を省略します。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
