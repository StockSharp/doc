# Adapter initialization: Delta Exchange India

The following code initializes [DeltaExchangeIndiaMessageAdapter](xref:StockSharp.DeltaExchangeIndia.DeltaExchangeIndiaMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DeltaExchangeIndiaMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_delta_exchange_india.md) page.

## See also

[Connector configuration](configuration_delta_exchange_india.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
