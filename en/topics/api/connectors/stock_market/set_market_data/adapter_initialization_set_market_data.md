# Adapter initialization: SET Market Data

The following code initializes [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new SetMarketDataMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_set_market_data.md) page.

## See also

[Connector configuration](configuration_set_market_data.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
