# Adapter initialization: 0x

The following code initializes [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Your API key>".To<SecureString>(),
	WalletAddress = "<Your wallet address>",
	PrivateKey = "<Your private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_zero_x.md) page.

## See also

[Connector configuration](configuration_zero_x.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
