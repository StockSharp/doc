# Initialization of Velo Data Adapter

The code below demonstrates how to initialize the [VeloDataMessageAdapter](xref:StockSharp.VeloData.VeloDataMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new VeloDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
