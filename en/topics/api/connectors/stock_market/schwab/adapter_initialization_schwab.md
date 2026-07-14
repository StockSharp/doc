# Charles Schwab adapter initialization

The following code demonstrates how to initialize [SchwabMessageAdapter](xref:StockSharp.Schwab.SchwabMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SchwabMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<access token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
