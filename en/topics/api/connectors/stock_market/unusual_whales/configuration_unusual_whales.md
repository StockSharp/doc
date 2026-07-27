# Connector configuration: Unusual Whales

Configure the following properties before connecting to Unusual Whales. The list is verified against [UnusualWhalesMessageAdapter](xref:StockSharp.UnusualWhales.UnusualWhalesMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `CandleLimit` (`int`)
- `NewsLimit` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `UnusualFlowOnly` (`bool`)
- `OtmMarketTide` (`bool`)
- `FiveMinuteMarketTide` (`bool`)

## See also

[Graphical configuration](graphical_configuration_unusual_whales.md)

[Adapter initialization](adapter_initialization_unusual_whales.md)
