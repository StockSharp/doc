# Adapter initialisieren: Chainflip

Der folgende Code initialisiert [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new ChainflipMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihre EVM-Wallet-Adresse>",
	PrivateKey = "<Ihr privater EVM-Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Wallet-Zugangsdaten, Zieladressen und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_chainflip.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_chainflip.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
