# Initialization of DXtrade Adapter

The code below demonstrates how to initialize the [DXtradeMessageAdapter](xref:StockSharp.DXtrade.DXtradeMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DXtradeMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your API Key>".To<SecureString>(),
	Secret = "<Your API Secret>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
							
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
