# Connector configuration: MarketData.app

Configure the following properties before connecting to MarketData.app. The list is verified against [MarketDataAppMessageAdapter](xref:StockSharp.MarketDataApp.MarketDataAppMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `RestEndpoint` (`Uri`)

## Advanced settings

These properties control provider endpoints, request pacing, filters, data options, and result limits.

- `ExtendedHours` (`bool`)
- `AdjustSplits` (`bool`)
- `MaximumOptionContracts` (`int`)

## See also

[Graphical configuration](graphical_configuration_marketdataapp.md)

[Adapter initialization](adapter_initialization_marketdataapp.md)
