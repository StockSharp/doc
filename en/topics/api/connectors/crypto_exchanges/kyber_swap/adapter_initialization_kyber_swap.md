# Adapter initialization: KyberSwap

The following code initializes [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter) and adds it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector connector = new Connector();
...
var messageAdapter = new KyberSwapMessageAdapter(connector.TransactionIdGenerator)
{
	ClientId = "<Your client ID>",
	WalletAddress = "<Your wallet address>",
	PrivateKey = "<Your private key>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Set the credentials and any other required properties described on the [Connector configuration](configuration_kyber_swap.md) page.

## See also

[Connector configuration](configuration_kyber_swap.md)

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
