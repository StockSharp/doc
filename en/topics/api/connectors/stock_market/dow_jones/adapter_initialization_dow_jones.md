# Adapter initialization Dow Jones

The code below demonstrates how to initialize the [DowJonesMessageAdapter](xref:StockSharp.DowJones.DowJonesMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new DowJonesMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Your value>".To<SecureString>(),
	ClientId = "<Your value>",
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
