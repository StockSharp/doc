# Connector configuration: Quiver Quantitative

Configure the following properties before connecting to Quiver Quantitative. The list is verified against [QuiverQuantMessageAdapter](xref:StockSharp.QuiverQuant.QuiverQuantMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PageSize` (`int`)
- `MaxPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `LimitInsiderCodes` (`bool`)
- `MostRecentInstitutional` (`bool`)
- `IncludeNewFunds` (`bool`)
- `CorporateDonorCycle` (`string`)

## See also

[Graphical configuration](graphical_configuration_quiver_quant.md)

[Adapter initialization](adapter_initialization_quiver_quant.md)
