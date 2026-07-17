# Adapter initialization: dxFeed

The following code shows how to initialize [DxFeedMessageAdapter](xref:StockSharp.DxFeed.DxFeedMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DxFeedMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<value>".ToSecureString(),
	Address = "<value>",
	MarketDepthSources = "<value>",
	AggregationPeriod = TimeSpan.FromSeconds(10),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
