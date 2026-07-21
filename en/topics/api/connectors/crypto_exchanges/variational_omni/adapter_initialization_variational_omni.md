# Initialization of Variational Omni Adapter

The code below demonstrates how to initialize the [VariationalOmniMessageAdapter](xref:StockSharp.VariationalOmni.VariationalOmniMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new VariationalOmniMessageAdapter(Connector.TransactionIdGenerator)
{
	Endpoint = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
