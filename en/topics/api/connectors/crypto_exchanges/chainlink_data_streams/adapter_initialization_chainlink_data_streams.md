# Initialization of Chainlink Data Streams Adapter

The code below demonstrates how to initialize the [ChainlinkDataStreamsMessageAdapter](xref:StockSharp.ChainlinkDataStreams.ChainlinkDataStreamsMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new ChainlinkDataStreamsMessageAdapter(Connector.TransactionIdGenerator)
{
	Key = "<Your value>".To<SecureString>(),
	Secret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
