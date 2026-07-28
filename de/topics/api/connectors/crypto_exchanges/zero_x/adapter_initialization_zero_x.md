# Adapter initialisieren: 0x

Der folgende Code initialisiert [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new ZeroXMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr API-Schlüssel>".To<SecureString>(),
	WalletAddress = "<Ihre Wallet-Adresse>",
	PrivateKey = "<Ihr privater Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_zero_x.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_zero_x.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
