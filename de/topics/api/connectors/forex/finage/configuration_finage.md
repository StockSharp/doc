# Connector-Konfiguration: Finage

Konfigurieren Sie vor der Verbindung mit Finage die folgenden Adaptereigenschaften. Die Liste wurde anhand von [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ApiKey` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern REST- und WebSocket-Zugriff, Endpunkte, Marktdatenoptionen, Symbolfilter und Ergebnislimits.

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_finage.md)

[Adapterinitialisierung](adapter_initialization_finage.md)
