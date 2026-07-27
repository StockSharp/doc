# Connector-Konfiguration: Bigul

Konfigurieren Sie vor der Verbindung mit Bigul die folgenden Adaptereigenschaften. Die Liste wurde anhand von [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_bigul.md)

[Adapterinitialisierung](adapter_initialization_bigul.md)
