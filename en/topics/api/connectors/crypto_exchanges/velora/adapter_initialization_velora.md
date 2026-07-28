# Adapter initialization: Velora

The following code initializes [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<Your partner ID>",
	WalletAddress = "<Your wallet address>",
	PrivateKey = "<Your private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_velora.md) page.

## See also

[Connector configuration](configuration_velora.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
