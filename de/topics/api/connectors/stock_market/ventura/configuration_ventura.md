# Connector-Konfiguration: Ventura

Konfigurieren Sie vor der Verbindung mit Ventura die folgenden Adaptereigenschaften. Die Liste wurde anhand von [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ClientId` (`string`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `RequestToken` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `Pin` (`SecureString`)
- `TotpSecret` (`SecureString`)
- `MacAddress` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`VenturaProducts`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `RestAddress` (`Uri`)
- `MarketDataAddress` (`Uri`)
- `OrderStatusAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_ventura.md)

[Adapterinitialisierung](adapter_initialization_ventura.md)
