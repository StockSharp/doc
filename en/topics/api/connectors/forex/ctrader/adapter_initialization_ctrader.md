# Initialization of cTrader Adapter

The code below demonstrates how to initialize the [cTraderMessageAdapter](xref:StockSharp.cTrader.cTraderMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new cTraderMessageAdapter(Connector.TransactionIdGenerator)
{
    IsDemo = true, // Demo
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)