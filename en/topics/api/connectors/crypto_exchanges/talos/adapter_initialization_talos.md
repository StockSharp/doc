# Initialization of Talos Adapter

The code below demonstrates how to initialize the [TalosMessageAdapter](xref:StockSharp.Talos.TalosMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new TalosMessageAdapter(Connector.TransactionIdGenerator)
{
	Address = "<Your value>".To<EndPoint>(),
	SenderCompId = "<Your value>",
	TargetCompId = "<Your value>",
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
