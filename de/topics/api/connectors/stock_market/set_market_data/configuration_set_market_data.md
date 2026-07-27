# Connector-Konfiguration: SET Market Data

Konfigurieren Sie vor der Verbindung mit SET Market Data die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_set_market_data.md)

[Adapterinitialisierung](adapter_initialization_set_market_data.md)
