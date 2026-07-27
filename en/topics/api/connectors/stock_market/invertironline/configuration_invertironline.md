# Connector configuration: InvertirOnline

Configure the following properties before connecting to InvertirOnline. The list is verified against [InvertirOnlineMessageAdapter](xref:StockSharp.InvertirOnline.InvertirOnlineMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `PortfolioName` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultCountry` (`InvertirOnlineCountries`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`InvertirOnlineSettlements`)
- `AdjustedHistory` (`bool`)
- `MarketDataPollingInterval` (`TimeSpan`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_invertironline.md)

[Adapter initialization](adapter_initialization_invertironline.md)
