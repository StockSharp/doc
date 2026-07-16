# FYERS adapter initialization

The following code demonstrates how to initialize [FyersMessageAdapter](xref:StockSharp.Fyers.FyersMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new FyersMessageAdapter(Connector.TransactionIdGenerator)
{
	ClientId = "<Client ID>",
	Token = "<Token>".ToSecureString(),
	DefaultProduct = FyersProducts.Intraday,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)

