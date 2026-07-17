# Upstox adapter initialization

The following code demonstrates how to initialize [UpstoxMessageAdapter](xref:StockSharp.Upstox.UpstoxMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new UpstoxMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<Token>".ToSecureString(),
	IsDemo = true,
	DefaultProduct = UpstoxProducts.Delivery,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the example values with credentials and endpoints issued for your account.

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
