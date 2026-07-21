# Initialization of Extended Adapter

The code below demonstrates how to initialize the [ExtendedMessageAdapter](xref:StockSharp.Extended.ExtendedMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ExtendedMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	PrivateKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
