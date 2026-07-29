# Connector-Konfiguration: OpenFIGI

Konfigurieren Sie vor der Verbindung mit OpenFIGI die folgenden Adaptereigenschaften. Die Liste wurde anhand von [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Anbieterendpunkte, Anfrageabstände, Filter, Datenoptionen und Ergebnislimits.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_openfigi.md)

[Adapterinitialisierung](adapter_initialization_openfigi.md)
