# TradeZero adapter initialization

The following code demonstrates how to initialize [TradeZeroMessageAdapter](xref:StockSharp.TradeZero.TradeZeroMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new TradeZeroMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<TZ-API-KEY-ID>".ToSecureString(),
	Secret = "<TZ-API-SECRET-KEY>".ToSecureString(),
	DefaultRoute = "<optional order route>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

`DefaultRoute` can be omitted to let the connector select a compatible route.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
