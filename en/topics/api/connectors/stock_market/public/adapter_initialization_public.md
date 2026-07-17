# Public.com adapter initialization

The following code demonstrates how to initialize [PublicMessageAdapter](xref:StockSharp.Public.PublicMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new PublicMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
