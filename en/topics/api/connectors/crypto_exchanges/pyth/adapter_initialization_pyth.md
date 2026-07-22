# Initialization of Pyth Adapter

The code below demonstrates how to initialize the [PythMessageAdapter](xref:StockSharp.Pyth.PythMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PythMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
