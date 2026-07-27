# Adapter initialisieren: GuruFocus

Der folgende Code initialisiert [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new GuruFocusMessageAdapter(Connector.TransactionIdGenerator)
{
	Token = "<token>".ToSecureString(),
};

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_gurufocus.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_gurufocus.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
