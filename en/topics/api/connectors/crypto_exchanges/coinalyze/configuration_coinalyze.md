# Connector configuration: Coinalyze

Configure the following properties before connecting to Coinalyze. The list is verified against [CoinalyzeMessageAdapter](xref:StockSharp.Coinalyze.CoinalyzeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `MarketType` (`CoinalyzeMarketTypes`)
- `CandleMetric` (`CoinalyzeCandleMetrics`)
- `Exchange` (`string`)
- `ConvertToUsd` (`bool`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RequestInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_coinalyze.md)

[Adapter initialization](adapter_initialization_coinalyze.md)
