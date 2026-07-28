# Adapter initialisieren: BitoPro

Der folgende Code initialisiert [BitoProMessageAdapter](xref:StockSharp.BitoPro.BitoProMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new BitoProMessageAdapter(connector.TransactionIdGenerator)
{
	Email = "<Ihre E-Mail-Adresse>",
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Secret>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_bitopro.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_bitopro.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
