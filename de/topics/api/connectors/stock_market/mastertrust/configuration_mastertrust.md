# Connector-Konfiguration: Mastertrust

Konfigurieren Sie vor der Verbindung mit Mastertrust die folgenden Adaptereigenschaften. Die Liste wurde anhand von [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ClientId` (`string`)
- `OAuthClientId` (`string`)
- `OAuthClientSecret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `RedirectUri` (`Uri`)
- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`MastertrustProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_mastertrust.md)

[Adapterinitialisierung](adapter_initialization_mastertrust.md)
