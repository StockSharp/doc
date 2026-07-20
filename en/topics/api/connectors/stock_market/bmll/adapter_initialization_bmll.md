# Adapter initialization BMLL

The code below demonstrates how to initialize the [BmllMessageAdapter](xref:StockSharp.Bmll.BmllMessageAdapter) and send it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new BmllMessageAdapter(Connector.TransactionIdGenerator)
{
	Login = "<Your value>",
	Password = "<Your value>".To<SecureString>(),
	Token = "<Your value>".To<SecureString>(),
	ApiKey = "<Your value>".To<SecureString>(),
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
