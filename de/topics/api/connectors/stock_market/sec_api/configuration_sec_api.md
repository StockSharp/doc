# Connector-Konfiguration: SEC API

Konfigurieren Sie vor der Verbindung mit SEC API die folgenden Adaptereigenschaften. Die Liste wurde anhand von [SecApiMessageAdapter](xref:StockSharp.SecApi.SecApiMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `ActiveOnly` (`bool`)
- `DefaultExchange` (`string`)
- `FormTypes` (`string`)
- `ResultLimit` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_sec_api.md)

[Adapterinitialisierung](adapter_initialization_sec_api.md)
