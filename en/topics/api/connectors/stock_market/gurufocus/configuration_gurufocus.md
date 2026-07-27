# Connector configuration: GuruFocus

Configure the following properties before connecting to GuruFocus. The list is verified against [GuruFocusMessageAdapter](xref:StockSharp.GuruFocus.GuruFocusMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `Address` (`Uri`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RegionCode` (`string`)
- `PageSize` (`int`)
- `MaxLookupPages` (`int`)
- `DatasetLimit` (`int`)
- `NewsLimit` (`int`)
- `FilingFormType` (`string`)
- `GuruTradeActions` (`string`)

## See also

[Graphical configuration](graphical_configuration_gurufocus.md)

[Adapter initialization](adapter_initialization_gurufocus.md)
