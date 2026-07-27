# Bit2Me 适配器初始化

下面的代码演示如何初始化 [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter)，并将其传递给 [Connector](xref:StockSharp.Algo.Connector)。

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<您的 API 密钥>".To<SecureString>(),
	Secret = "<您的 API Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

如果只需要公开市场数据，请省略 `Key` 和 `Secret`。可以通过 `RestEndpoint` 和 `WebSocketEndpoint` 更改 REST 与 WebSocket 地址。

## 推荐内容

[连接设置窗口](../../../graphical_user_interface/connection_settings_window.md)
