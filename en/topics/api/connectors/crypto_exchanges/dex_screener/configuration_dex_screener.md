# Connector configuration: DEX Screener

Configure the following properties before connecting to DEX Screener. The list is verified against [DexScreenerMessageAdapter](xref:StockSharp.DexScreener.DexScreenerMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `RestEndpoint` (`string`)
- `ChainId` (`string`)
- `TokenAddress` (`string`)
- `SearchQuery` (`string`)
- `PriceInUsd` (`bool`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)

## See also

[Graphical configuration](graphical_configuration_dex_screener.md)

[Adapter initialization](adapter_initialization_dex_screener.md)
