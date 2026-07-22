# Initialization of Paxos Adapter

The code below demonstrates how to initialize the [PaxosMessageAdapter](xref:StockSharp.Paxos.PaxosMessageAdapter) and pass it to the [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new PaxosMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Your value>".To<SecureString>(),
	ClientSecret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
