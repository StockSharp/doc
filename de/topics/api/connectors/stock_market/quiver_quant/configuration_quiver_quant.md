# Connector-Konfiguration: Quiver Quantitative

Konfigurieren Sie vor der Verbindung mit Quiver Quantitative die folgenden Adaptereigenschaften. Die Liste wurde anhand von [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_quiver_quant.md)

[Adapterinitialisierung](adapter_initialization_quiver_quant.md)
