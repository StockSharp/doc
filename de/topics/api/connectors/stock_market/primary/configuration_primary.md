# Connector-Konfiguration: Primary

Konfigurieren Sie vor der Verbindung mit Primary die folgenden Adaptereigenschaften. Die Liste wurde anhand von [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_primary.md)

[Adapterinitialisierung](adapter_initialization_primary.md)
