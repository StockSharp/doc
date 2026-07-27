# Connector-Konfiguration: TPEx

Konfigurieren Sie vor der Verbindung mit TPEx die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_tpex.md)

[Adapterinitialisierung](adapter_initialization_tpex.md)
