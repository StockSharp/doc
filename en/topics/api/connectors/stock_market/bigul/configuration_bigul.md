# Connector configuration: Bigul

Configure the following properties before connecting to Bigul. The list is verified against [BigulMessageAdapter](xref:StockSharp.Bigul.BigulMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ClientCode` (`string`)
- `ApiKey` (`SecureString`)
- `ApiSecret` (`SecureString`)
- `OneTimePassword` (`SecureString`)
- `Token` (`SecureString`)
- `Source` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `DefaultProduct` (`BigulProducts`)
- `MarketProtection` (`decimal`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_bigul.md)

[Adapter initialization](adapter_initialization_bigul.md)
