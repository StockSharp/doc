# Adapter initialisieren: STON.fi

Der folgende Code initialisiert [StonFiMessageAdapter](xref:StockSharp.StonFi.StonFiMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new StonFiMessageAdapter(connector.TransactionIdGenerator)
{
	WalletAddress = "<Ihre TON-Wallet-Adresse>",
	Mnemonic = "<Ihre Mnemonik aus 24 Wörtern>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Wallet-Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_stonfi.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_stonfi.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
