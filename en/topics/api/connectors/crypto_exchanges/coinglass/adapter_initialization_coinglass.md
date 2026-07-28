# Adapter initialization: CoinGlass

The following code initializes [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinGlassMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Your access token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_coinglass.md) page.

## See also

[Connector configuration](configuration_coinglass.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
