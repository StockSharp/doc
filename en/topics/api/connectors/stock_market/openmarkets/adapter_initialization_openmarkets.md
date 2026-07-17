# Adapter initialization: OpenMarkets

The following code shows how to initialize [OpenMarketsMessageAdapter](xref:StockSharp.OpenMarkets.OpenMarketsMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new OpenMarketsMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientSecret = "<value>".ToSecureString(),
	ClientId = "<value>",
	AccountCode = "<value>",
	DataSource = "<value>",
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
