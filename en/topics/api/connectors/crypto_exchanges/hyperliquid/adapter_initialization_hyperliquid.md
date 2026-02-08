# Adapter initialization Hyperliquid

The code below demonstrates how to initialize [HyperliquidMessageAdapter](xref:StockSharp.Hyperliquid.HyperliquidMessageAdapter) and pass it to [Connector](xref:StockSharp.Algo.Connector).

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HyperliquidMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Your Wallet Address>",
	PrivateKey = "<Your Private Key>".To<SecureString>(),
	Section = HyperliquidSections.Derivatives,
	IsTestnet = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Recommended content

[Connection settings window](../../../graphical_user_interface/connection_settings_window.md)
