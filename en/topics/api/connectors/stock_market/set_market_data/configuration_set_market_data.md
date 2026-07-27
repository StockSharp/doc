# Connector configuration: SET Market Data

Configure the following properties before connecting to SET Market Data. The list is verified against [SetMarketDataMessageAdapter](xref:StockSharp.SetMarketData.SetMarketDataMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `DataMode` (`SetMarketDataModes`)
- `Markets` (`string`)
- `IndexSectors` (`string`)
- `SecurityTypeCodes` (`string`)
- `IncludeOddLots` (`bool`)
- `IncludeIndices` (`bool`)

## See also

[Graphical configuration](graphical_configuration_set_market_data.md)

[Adapter initialization](adapter_initialization_set_market_data.md)
