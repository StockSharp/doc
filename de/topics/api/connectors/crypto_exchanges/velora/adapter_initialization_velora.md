# Adapter initialisieren: Velora

Der folgende Code initialisiert [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new VeloraMessageAdapter(connector.TransactionIdGenerator)
{
	Partner = "<Ihre Partnerkennung>",
	WalletAddress = "<Ihre Wallet-Adresse>",
	PrivateKey = "<Ihr privater Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_velora.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_velora.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
