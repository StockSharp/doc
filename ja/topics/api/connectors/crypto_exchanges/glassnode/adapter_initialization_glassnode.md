# Glassnode アダプターの初期化

以下のコードは、[GlassnodeMessageAdapter](xref:StockSharp.Glassnode.GlassnodeMessageAdapter) を初期化し、それを [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示しています。

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GlassnodeMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<設定値>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
