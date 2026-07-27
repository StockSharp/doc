# Connector-Konfiguration: EXANTE

Konfigurieren Sie vor der Verbindung mit EXANTE die folgenden Adaptereigenschaften. Die Liste wurde anhand von [ExanteMessageAdapter](xref:StockSharp.Exante.ExanteMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `SummaryCurrency` (`string`)
- `PollingInterval` (`TimeSpan`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `MaxMarketDepth` (`int`)
- `HistoryRequestSize` (`int`)
- `LiveAddress` (`Uri`)
- `DemoAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_exante.md)

[Adapterinitialisierung](adapter_initialization_exante.md)
