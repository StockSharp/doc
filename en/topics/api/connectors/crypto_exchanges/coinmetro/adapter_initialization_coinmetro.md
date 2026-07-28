# Adapter initialization: Coinmetro

The following code initializes [CoinmetroMessageAdapter](xref:StockSharp.Coinmetro.CoinmetroMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinmetroMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Your access token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_coinmetro.md) page.

## See also

[Connector configuration](configuration_coinmetro.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
