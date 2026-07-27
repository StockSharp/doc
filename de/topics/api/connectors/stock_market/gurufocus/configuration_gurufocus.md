# Connector-Konfiguration: GuruFocus

Konfigurieren Sie vor der Verbindung mit GuruFocus die folgenden Adaptereigenschaften. Die Liste wurde anhand von [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RegionCode` (`string`)
- `PageSize` (`int`)
- `MaxLookupPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `FilingFormType` (`string`)
- `GuruTradeActions` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_gurufocus.md)

[Adapterinitialisierung](adapter_initialization_gurufocus.md)
