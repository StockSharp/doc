# Connector-Konfiguration: CoinGlass

Konfigurieren Sie vor der Verbindung mit CoinGlass die folgenden Adaptereigenschaften. Die Liste wurde anhand von [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_coinglass.md)

[Adapterinitialisierung](adapter_initialization_coinglass.md)
