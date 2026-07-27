# Connector-Konfiguration: Zebu

Konfigurieren Sie vor der Verbindung mit Zebu die folgenden Adaptereigenschaften. Die Liste wurde anhand von [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_zebu.md)

[Adapterinitialisierung](adapter_initialization_zebu.md)
