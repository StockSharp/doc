# Initialization of BitGo Adapter

The code below demonstrates how to initialize the [BitGoMessageAdapter](xref:StockSharp.BitGo.BitGoMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitGoMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
	Account = "<Your value>",
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
