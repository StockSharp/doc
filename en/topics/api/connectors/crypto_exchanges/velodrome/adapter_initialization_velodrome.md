# Adapter initialization: Velodrome

The following code initializes [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VelodromeMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Your wallet address>",
	PrivateKey = "<Your private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_velodrome.md) page.

## See also

[Connector configuration](configuration_velodrome.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
