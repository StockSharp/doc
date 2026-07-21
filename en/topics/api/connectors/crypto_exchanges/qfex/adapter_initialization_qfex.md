# Initialization of QFEX Adapter

The code below demonstrates how to initialize the [QFEXMessageAdapter](xref:StockSharp.QFEX.QFEXMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new QFEXMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>",
	Secret = "<Your value>".To<SecureString>(),
	AccountId = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
