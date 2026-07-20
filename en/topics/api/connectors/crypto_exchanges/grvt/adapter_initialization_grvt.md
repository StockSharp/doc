# Initialization of GRVT Adapter

The code below demonstrates how to initialize the [GrvtMessageAdapter](xref:StockSharp.Grvt.GrvtMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new GrvtMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	Secret = "<Your value>".To<SecureString>(),
	SubAccountId = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
