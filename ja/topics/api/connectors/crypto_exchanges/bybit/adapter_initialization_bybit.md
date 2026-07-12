# ByBit アダプターの初期化

以下のコードは、[ByBitMessageAdapter](xref:StockSharp.ByBit.ByBitMessageAdapter) を初期化し、[Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ByBitMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<APIキー>".To<SecureString>(),
	Secret = "<APIシークレット>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...

```

別のより便利な方法として、`AddAdapter<T>()` 拡張メソッドを使用できます:

```cs
Connector Connector = new Connector();
...
Connector.AddAdapter<ByBitMessageAdapter>(a =>
{
	a.Key = "<APIキー>".To<SecureString>();
	a.Secret = "<APIシークレット>".To<SecureString>();
});
```

## 関連項目

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
