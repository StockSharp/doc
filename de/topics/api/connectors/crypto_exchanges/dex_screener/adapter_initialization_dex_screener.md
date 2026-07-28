# Adapter initialisieren: DEX Screener

Der folgende Code initialisiert [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) und fügt ihn zu [Connector](xref:StockSharp.Algo.Connector) hinzu.

```cs
Connector connector = new Connector();
...
var messageAdapter = new DexScreenerMessageAdapter(connector.TransactionIdGenerator);
connector.Adapter.InnerAdapters.Add(messageAdapter);
...
```

Tragen Sie die Zugangsdaten und alle weiteren erforderlichen Eigenschaften ein, die auf der Seite [Connector-Konfiguration](configuration_dex_screener.md) beschrieben sind.

## Siehe auch

[Connector-Konfiguration](configuration_dex_screener.md)

[Fenster für Verbindungseinstellungen](../../../graphical_user_interface/connection_settings_window.md)
