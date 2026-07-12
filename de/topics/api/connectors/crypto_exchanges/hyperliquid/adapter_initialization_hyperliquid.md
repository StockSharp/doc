# Adapterinitialisierung Hyperliquid

Der folgende Code zeigt, wie der [HyperliquidMessageAdapter](xref:StockSharp.Hyperliquid.HyperliquidMessageAdapter) initialisiert und an [Connector](xref:StockSharp.Algo.Connector) übergeben wird.

```cs
Connector Connector = new Connector();
...
var messageAdapter = new HyperliquidMessageAdapter(Connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihre Wallet-Adresse>",
	PrivateKey = "<Ihr privater Schlüssel>".To<SecureString>(),
	Section = HyperliquidSections.Derivatives,
	IsTestnet = false,
};
Connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

## Empfohlene Inhalte

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
