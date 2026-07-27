# Connector configuration: Wisdom Capital

Configure the following properties before connecting to Wisdom Capital. The list is verified against [WisdomCapitalMessageAdapter](xref:StockSharp.WisdomCapital.WisdomCapitalMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `MarketDataKey` (`SecureString`)
- `MarketDataSecret` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

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

## See also

[Graphical configuration](graphical_configuration_wisdom_capital.md)

[Adapter initialization](adapter_initialization_wisdom_capital.md)
