# Initialization of Bitpanda Fusion Adapter

The code below demonstrates how to initialize the [BitpandaFusionMessageAdapter](xref:StockSharp.BitpandaFusion.BitpandaFusionMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BitpandaFusionMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
