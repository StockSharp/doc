# Adapter initialization: Dexalot

The following code initializes [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexalotMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Your wallet address>",
	PrivateKey = "<Your private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the wallet credentials and any other required properties described on the [Connector configuration](configuration_dexalot.md) page.

## See also

[Connector configuration](configuration_dexalot.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
