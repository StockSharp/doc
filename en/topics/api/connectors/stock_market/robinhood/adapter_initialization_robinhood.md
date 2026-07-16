# Robinhood adapter initialization

The following code demonstrates how to initialize [RobinhoodMessageAdapter](xref:StockSharp.Robinhood.RobinhoodMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new RobinhoodMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	Address = new Uri("https://agent.robinhood.com/mcp/trading"),
	PollingInterval = TimeSpan.FromSeconds(2),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

