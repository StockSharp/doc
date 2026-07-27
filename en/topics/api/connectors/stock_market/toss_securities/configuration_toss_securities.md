# Connector configuration: Toss Securities

Configure the following properties before connecting to Toss Securities. The list is verified against [TossSecuritiesMessageAdapter](xref:StockSharp.TossSecurities.TossSecuritiesMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AccountSequence` (`long`)
- `PollingInterval` (`TimeSpan`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `AdjustedCandles` (`bool`)
- `RestAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_toss_securities.md)

[Adapter initialization](adapter_initialization_toss_securities.md)
