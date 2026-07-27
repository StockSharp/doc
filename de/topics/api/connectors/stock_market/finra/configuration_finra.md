# Connector-Konfiguration: FINRA

Konfigurieren Sie vor der Verbindung mit FINRA die folgenden Adaptereigenschaften. Die Liste wurde anhand von [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_finra.md)

[Adapterinitialisierung](adapter_initialization_finra.md)
