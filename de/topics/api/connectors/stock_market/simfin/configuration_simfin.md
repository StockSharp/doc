# Connector-Konfiguration: SimFin

Konfigurieren Sie vor der Verbindung mit SimFin die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anbieterendpunkte, Anfrageabstände, Filter, Datenoptionen und Ergebnislimits.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_simfin.md)

[Adapterinitialisierung](adapter_initialization_simfin.md)
