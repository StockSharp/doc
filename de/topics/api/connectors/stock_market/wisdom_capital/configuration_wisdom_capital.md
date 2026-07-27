# Connector-Konfiguration: Wisdom Capital

Konfigurieren Sie vor der Verbindung mit Wisdom Capital die folgenden Adaptereigenschaften. Die Liste wurde anhand von [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter) geprüft.

## Grundeinstellungen

Diese Einstellungen werden im Verbindungseditor zuerst angezeigt.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `MarketDataKey` (`SecureString`)
- `MarketDataSecret` (`SecureString`)

## Erweiterte Einstellungen

Diese Eigenschaften steuern Endpunkte, Filter, Grenzwerte und weitere anbieterspezifische Optionen.

- `Token` (`SecureString`)
- `UserId` (`string`)
- `MarketDataToken` (`SecureString`)
- `MarketDataUserId` (`string`)
- `PortfolioName` (`string`)
- `DefaultProduct` (`WisdomCapitalProducts`)
- `Source` (`string`)
- `PollingInterval` (`TimeSpan`)
- `ReconnectAttempts` (`int`)
- `EngineIoVersion` (`int`)
- `RestAddress` (`Uri`)

## Siehe auch

[Grafische Konfiguration](graphical_configuration_wisdom_capital.md)

[Adapterinitialisierung](adapter_initialization_wisdom_capital.md)
