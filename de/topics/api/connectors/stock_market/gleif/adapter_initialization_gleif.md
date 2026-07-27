# Adapter initialisieren: GLEIF

Der folgende Code initialisiert [GleifMessageAdapter](xref:StockSharp.Gleif.GleifMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new GleifMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_gleif.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_gleif.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
