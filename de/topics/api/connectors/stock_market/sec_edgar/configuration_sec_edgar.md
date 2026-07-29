# Connector-Konfiguration: SEC EDGAR

Konfigurieren Sie vor der Verbindung mit SEC EDGAR die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anbieterendpunkte, Anfrageabstände, Filter, Datenoptionen und Ergebnislimits.

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_sec_edgar.md)

[Adapterinitialisierung](adapter_initialization_sec_edgar.md)
