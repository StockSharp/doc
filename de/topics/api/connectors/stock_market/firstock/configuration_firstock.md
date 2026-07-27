# Connector-Konfiguration: Firstock

Konfigurieren Sie vor der Verbindung mit Firstock die folgenden Adaptereigenschaften. Die Liste wurde anhand von [FirstockMessageAdapter](xref:StockSharp.Firstock.FirstockMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `UserId` (`string`)
- `Password` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `VendorCode` (`string`)
- `ApiKey` (`SecureString`)
- `Token` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `PortfolioName` (`string`)
- `DefaultProduct` (`FirstockProducts`)
- `MarketProtection` (`decimal`)
- `PriceDivisor` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `SymbolsAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_firstock.md)

[Adapterinitialisierung](adapter_initialization_firstock.md)
