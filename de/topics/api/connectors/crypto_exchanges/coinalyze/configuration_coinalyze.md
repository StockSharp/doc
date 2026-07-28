# Connector-Konfiguration: Coinalyze

Konfigurieren Sie vor der Verbindung mit Coinalyze die folgenden Adaptereigenschaften. Die Liste wurde anhand von [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_coinalyze.md)

[Adapterinitialisierung](adapter_initialization_coinalyze.md)
