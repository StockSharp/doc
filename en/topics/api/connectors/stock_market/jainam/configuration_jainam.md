# Connector configuration: Jainam

Configure the following properties before connecting to Jainam. The list is verified against [JainamMessageAdapter](xref:StockSharp.Jainam.JainamMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `UserId` (`string`)
- `AppCode` (`string`)
- `ApiSecret` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `DefaultProduct` (`JainamProducts`)
- `ReconnectAttempts` (`int`)
- `PollingInterval` (`TimeSpan`)
- `RestAddress` (`Uri`)
- `InstrumentAddress` (`string`)
- `WebSocketAddress` (`string`)

## See also

[Graphical configuration](graphical_configuration_jainam.md)

[Adapter initialization](adapter_initialization_jainam.md)
