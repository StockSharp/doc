# Connector-Konfiguration: Open DART

Konfigurieren Sie vor der Verbindung mit Open DART die folgenden Adaptereigenschaften. Die Liste wurde anhand von [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_open_dart.md)

[Adapterinitialisierung](adapter_initialization_open_dart.md)
