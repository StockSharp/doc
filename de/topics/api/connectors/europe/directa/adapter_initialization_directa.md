# Adapter initialisieren: Directa

Der folgende Code initialisiert [DirectaMessageAdapter](xref:StockSharp.Directa.DirectaMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
var messageAdapter = new DirectaMessageAdapter(Connector.TransactionIdGenerator);

Connector.Adapter.InnerAdapters.Add(messageAdapter);
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_directa.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_directa.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
