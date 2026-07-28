# Adapter initialisieren: AscendEX

Der folgende Code initialisiert [AscendExMessageAdapter](xref:StockSharp.AscendEx.AscendExMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new AscendExMessageAdapter(connector.TransactionIdGenerator)
{
	Key = "<Ihr API-Schlüssel>".To<SecureString>(),
	Secret = "<Ihr API-Secret>".To<SecureString>(),
	AccountGroup = 0,
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_ascendex.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_ascendex.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
