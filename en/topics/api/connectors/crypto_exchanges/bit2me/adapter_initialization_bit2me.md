# Initialization of Bit2Me Adapter

The code below demonstrates how to initialize the [Bit2MeMessageAdapter](xref:StockSharp.Bit2Me.Bit2MeMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new Bit2MeMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Omit `Key` and `Secret` when only public market data is required. REST and WebSocket addresses can be changed through `RestEndpoint` and `WebSocketEndpoint`.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
