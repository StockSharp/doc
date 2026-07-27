# Adapter initialisieren: TPEx

Der folgende Code initialisiert [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new TpexMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_tpex.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_tpex.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
