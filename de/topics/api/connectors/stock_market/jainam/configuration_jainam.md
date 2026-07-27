# Connector-Konfiguration: Jainam

Konfigurieren Sie vor der Verbindung mit Jainam die folgenden Adaptereigenschaften. Die Liste wurde anhand von [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_jainam.md)

[Adapterinitialisierung](adapter_initialization_jainam.md)
