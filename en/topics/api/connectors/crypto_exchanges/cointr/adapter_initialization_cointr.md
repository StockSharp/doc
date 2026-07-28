# Adapter initialization: CoinTR

The following code initializes [CoinTRMessageAdapter](xref:StockSharp.CoinTR.CoinTRMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new CoinTRMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Your API key>".To<SecureString>(),
	Secret = "<Your API secret>".To<SecureString>(),
	Passphrase = "<Your API passphrase>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_cointr.md) page.

## See also

[Connector configuration](configuration_cointr.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
