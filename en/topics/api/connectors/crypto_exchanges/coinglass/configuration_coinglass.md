# Connector configuration: CoinGlass

Configure the following properties before connecting to CoinGlass. The list is verified against [CoinGlassMessageAdapter](xref:StockSharp.CoinGlass.CoinGlassMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinGlassMarketTypes`)
- `CandleMetric` (`CoinGlassCandleMetrics`)
- `Exchange` (`string`)
- `Symbol` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_coinglass.md)

[Adapter initialization](adapter_initialization_coinglass.md)
