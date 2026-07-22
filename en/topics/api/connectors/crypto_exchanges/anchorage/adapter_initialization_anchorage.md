# Initialization of Anchorage Adapter

The code below demonstrates how to initialize the [AnchorageMessageAdapter](xref:StockSharp.Anchorage.AnchorageMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new AnchorageMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<Your value>".To<SecureString>(),
	SigningKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
