# Adapter initialization: Chainflip

The following code initializes [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Your EVM wallet address>",
	PrivateKey = "<Your EVM private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the wallet credentials, destination addresses, and any other required properties described on the [Connector configuration](configuration_chainflip.md) page.

## See also

[Connector configuration](configuration_chainflip.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
