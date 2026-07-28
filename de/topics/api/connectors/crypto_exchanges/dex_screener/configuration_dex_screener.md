# Connector-Konfiguration: DEX Screener

Konfigurieren Sie vor der Verbindung mit DEX Screener die folgenden Adaptereigenschaften. Die Liste wurde anhand von [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_dex_screener.md)

[Adapterinitialisierung](adapter_initialization_dex_screener.md)
