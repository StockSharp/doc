# Adapter initialisieren: Finage

Der folgende Code initialisiert [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new FinageMessageAdapter(connector.TransactionIdGenerator)
{
	ApiKey = "<Ihr API-Schlüssel>".To<SecureString>(),
	StreamingToken = "<Ihr Streaming-Token>".To<SecureString>(),
};
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugriffswerte und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_finage.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_finage.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
