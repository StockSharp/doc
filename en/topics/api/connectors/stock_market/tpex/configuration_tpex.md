# Connector configuration: TPEx

Configure the following properties before connecting to TPEx. The list is verified against [TpexMessageAdapter](xref:StockSharp.Tpex.TpexMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Address` (`Uri`)
- `Market` (`TpexMarkets`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `IncludeListedDerivatives` (`bool`)
- `IncludeValuations` (`bool`)
- `CacheTimeout` (`TimeSpan`)
- `MaxHistoryMonths` (`int`)

## See also

[Graphical configuration](graphical_configuration_tpex.md)

[Adapter initialization](adapter_initialization_tpex.md)
