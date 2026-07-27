# Connector-Konfiguration: KRX Open API

Konfigurieren Sie vor der Verbindung mit KRX Open API die folgenden Adaptereigenschaften. Die Liste wurde anhand von [KrxOpenApiMessageAdapter](xref:StockSharp.KrxOpenApi.KrxOpenApiMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`KrxDataSets`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `MaxRequests` (`int`)
- `Address` (`Uri`)
- `SampleAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_krx_open_api.md)

[Adapterinitialisierung](adapter_initialization_krx_open_api.md)
