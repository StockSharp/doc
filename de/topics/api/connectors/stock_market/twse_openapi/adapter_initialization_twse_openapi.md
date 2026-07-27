# Adapter initialisieren: TWSE

Der folgende Code initialisiert [TwseMessageAdapter](xref:StockSharp.Twse.TwseMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new TwseMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_twse_openapi.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_twse_openapi.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
