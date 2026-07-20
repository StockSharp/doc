# Adapter initialization J.P. Morgan DataQuery

The code below demonstrates how to initialize the [JpmDataQueryMessageAdapter](xref:StockSharp.J.P. Morgan DataQuery.JpmDataQueryMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new JpmDataQueryMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Your value>",
	ClientSecret = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
