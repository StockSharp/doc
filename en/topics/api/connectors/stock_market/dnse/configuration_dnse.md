# Connector configuration: DNSE

Configure the following properties before connecting to DNSE. The list is verified against [DnseMessageAdapter](xref:StockSharp.Dnse.DnseMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `TradingToken` (`SecureString`)
- `Account` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `OtpType` (`DnseOtpTypes`)
- `OneTimePassword` (`SecureString`)
- `RequestEmailOtpOnConnect` (`bool`)
- `DefaultLoanPackageId` (`int`)
- `DefaultBoardId` (`string`)
- `MarketDataPriceMultiplier` (`decimal`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `ApiVersion` (`string`)
- `DateHeaderName` (`string`)
- `RestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_dnse.md)

[Adapter initialization](adapter_initialization_dnse.md)
