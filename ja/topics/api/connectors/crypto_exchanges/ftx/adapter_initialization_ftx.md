> [!CAUTION]
> **FTX 取引所は運営を終了しました。このコネクターは動作しません。ドキュメントは参照用としてのみ保持されています。**

# FTX アダプターの初期化

以下のコードは、[FtxMessageAdapter](xref:StockSharp.FTX.FtxMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new FtxMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<設定値>".To<SecureString>(),
	Secret = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
