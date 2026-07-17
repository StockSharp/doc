# Adapter initialization: kabu Station

The following code shows how to initialize [KabuStationMessageAdapter](xref:StockSharp.KabuStation.KabuStationMessageAdapter) and add it to [Connector](xref:StockSharp.Algo.Connector).

```cs
var messageAdapter = new KabuStationMessageAdapter(Connector.TransactionIdGenerator)
{
	ApiPassword = "<value>".ToSecureString(),
	IsDemo = true,
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Replace the sample values with the parameters issued or configured for your account.

## See also

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
