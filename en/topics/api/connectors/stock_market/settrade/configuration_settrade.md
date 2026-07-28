# Connector configuration: Settrade

Configure the following properties before connecting to Settrade. The list is verified against [SettradeMessageAdapter](xref:StockSharp.Settrade.SettradeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AppCode` (`string`)
- `BrokerId` (`string`)
- `Account` (`string`)
- `Pin` (`SecureString`)
- `AccountType` (`SettradeAccountTypes`)
- `IsDemo` (`bool`)

## Advanced settings

These properties control login parameters, production and sandbox endpoints, and private-state polling.

- `LoginParameters` (`string`)
- `RestEndpoint` (`string`)
- `DemoRestEndpoint` (`string`)
- `MarketDataEndpoint` (`string`)
- `DemoMarketDataEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_settrade.md)

[Adapter initialization](adapter_initialization_settrade.md)
