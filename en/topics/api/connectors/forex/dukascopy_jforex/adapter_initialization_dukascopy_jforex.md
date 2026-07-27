# Adapter initialization: DukasCopy JForex

The following code shows how to initialize [DukasCopyJForexMessageAdapter](xref:StockSharp.DukasCopyJForex.DukasCopyJForexMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new DukasCopyJForexMessageAdapter(Connector.TransactionIdGenerator)
{
	Password = "<value>".ToSecureString(),
	Login = "<value>",
	BridgeJarPath = "<value>",
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
