# Adapter initialisieren: TraderMade

Der folgende Code initialisiert [TraderMadeMessageAdapter](xref:StockSharp.TraderMade.TraderMadeMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new TraderMadeMessageAdapter(connector.TransactionIdGenerator)
{
	RestKey = "<Ihr REST-API-Schlüssel>".To<SecureString>(),
	StreamingKey = "<Ihr Streaming-API-Schlüssel>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_tradermade.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_tradermade.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
