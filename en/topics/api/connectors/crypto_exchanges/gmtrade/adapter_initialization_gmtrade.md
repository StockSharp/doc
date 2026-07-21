# Initialization of GMTrade Adapter

The code below demonstrates how to initialize the [GMTradeMessageAdapter](xref:StockSharp.GMTrade.GMTradeMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GMTradeMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
