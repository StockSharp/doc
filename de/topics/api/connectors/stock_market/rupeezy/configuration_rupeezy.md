# Connector-Konfiguration: Rupeezy

Konfigurieren Sie vor der Verbindung mit Rupeezy die folgenden Adaptereigenschaften. Die Liste wurde anhand von [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_rupeezy.md)

[Adapterinitialisierung](adapter_initialization_rupeezy.md)
