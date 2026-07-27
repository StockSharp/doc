# Connector configuration: Marketaux

Configure the following properties before connecting to Marketaux. The list is verified against [MarketauxMessageAdapter](xref:StockSharp.Marketaux.MarketauxMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Languages` (`string`)
- `EntityTypes` (`string`)
- `Countries` (`string`)
- `MustHaveEntities` (`bool`)
- `GroupSimilar` (`bool`)
- `NewsPageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `SentimentInterval` (`MarketauxIntervals`)

## See also

[Graphical configuration](graphical_configuration_marketaux.md)

[Adapter initialization](adapter_initialization_marketaux.md)
