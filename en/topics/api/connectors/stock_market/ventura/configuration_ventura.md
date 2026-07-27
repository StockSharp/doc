# Connector configuration: Ventura

Configure the following properties before connecting to Ventura. The list is verified against [VenturaMessageAdapter](xref:StockSharp.Ventura.VenturaMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `ClientId` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

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

## See also

[Graphical configuration](graphical_configuration_ventura.md)

[Adapter initialization](adapter_initialization_ventura.md)
