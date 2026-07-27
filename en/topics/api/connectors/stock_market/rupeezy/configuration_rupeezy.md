# Connector configuration: Rupeezy

Configure the following properties before connecting to Rupeezy. The list is verified against [RupeezyMessageAdapter](xref:StockSharp.Rupeezy.RupeezyMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ApplicationId` (`string`)
- `ApiKey` (`SecureString`)
- `AuthCode` (`SecureString`)
- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `DefaultProduct` (`RupeezyProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_rupeezy.md)

[Adapter initialization](adapter_initialization_rupeezy.md)
