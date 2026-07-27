# Connector-Konfiguration: Korean FSC

Konfigurieren Sie vor der Verbindung mit Korean FSC die folgenden Adaptereigenschaften. Die Liste wurde anhand von [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_korean_fsc.md)

[Adapterinitialisierung](adapter_initialization_korean_fsc.md)
