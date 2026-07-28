# Adapter initialization: CoinSpot

The following code initializes [CoinSpotMessageAdapter](xref:StockSharp.CoinSpot.CoinSpotMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinSpotMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_coinspot.md) page.

## See also

[Connector configuration](configuration_coinspot.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
