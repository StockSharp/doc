# Adapter initialisieren: Birdeye

Der folgende Code initialisiert [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new BirdeyeMessageAdapter(connector.TransactionIdGenerator)
{
	Token = "<Ihr Zugriffstoken>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_birdeye.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_birdeye.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
