# Adapter initialisieren: SEC API

Der folgende Code initialisiert [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new SecApiMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_sec_api.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_sec_api.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
