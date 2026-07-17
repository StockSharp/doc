# Adapter initialization: Trading 212

The following code shows how to initialize [Trading212MessageAdapter](xref:StockSharp.Trading212.Trading212MessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new Trading212MessageAdapter(Connector.TransactionIdGenerator)
{
	ApiKey = "<value>".ToSecureString(),
	ApiSecret = "<value>".ToSecureString(),
	IsDemo = true,
	PollingInterval = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
