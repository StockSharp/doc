# Bit2Me アダプターの初期化

次のコードは、[Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) を初期化して [Connector](xref:StockSharp.Algo.Connector) に渡す方法を示します。

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<API キー>".To<SecureString>(),
	Secret = "<API シークレット>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

公開市場データのみが必要な場合は `Key` と `Secret` を省略します。REST と WebSocket のアドレスは `RestEndpoint` と `WebSocketEndpoint` で変更できます。

## 推奨コンテンツ

[接続設定ウィンドウ](../../../graphical_user_interface/connection_settings_window.md)
