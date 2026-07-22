# Initialization of Copper Adapter

The code below demonstrates how to initialize the [CopperMessageAdapter](xref:StockSharp.Copper.CopperMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new CopperMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>",
	ApiSecret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
