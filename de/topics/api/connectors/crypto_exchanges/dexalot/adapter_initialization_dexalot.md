# Adapter initialisieren: Dexalot

Der folgende Code initialisiert [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexalotMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihre Wallet-Adresse>",
	PrivateKey = "<Ihr privater Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Wallet-Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_dexalot.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_dexalot.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
