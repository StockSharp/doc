# Connector configuration: Tradejini

Configure the following properties before connecting to Tradejini. The list is verified against [TradejiniMessageAdapter](xref:StockSharp.Tradejini.TradejiniMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ApiKey` (`SecureString`)
- `Password` (`SecureString`)
- `TwoFactorCode` (`SecureString`)
- `TwoFactorType` (`TradejiniTwoFactorTypes`)
- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `DefaultProduct` (`TradejiniProducts`)
- `Address` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_tradejini.md)

[Adapter initialization](adapter_initialization_tradejini.md)
