# Connector-Konfiguration: TASE Data Hub

Konfigurieren Sie vor der Verbindung mit TASE Data Hub die folgenden Adaptereigenschaften. Die Liste wurde anhand von [TaseDataHubMessageAdapter](xref:StockSharp.TaseDataHub.TaseDataHubMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Address` (`Uri`)
- `Scope` (`string`)
- `SecurityLookupDays` (`int`)
- `ReferenceCacheTimeout` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_tase_data_hub.md)

[Adapterinitialisierung](adapter_initialization_tase_data_hub.md)
