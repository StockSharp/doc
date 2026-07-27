# Connector configuration: Korean FSC

Configure the following properties before connecting to Korean FSC. The list is verified against [KoreanFscMessageAdapter](xref:StockSharp.KoreanFsc.KoreanFscMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `DataSet` (`KoreanFscDataSets`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `Market` (`KoreanFscMarkets`)
- `ReferenceDate` (`DateTime?`)
- `LatestSearchDays` (`int`)
- `PageSize` (`int`)
- `MaxPages` (`int`)

## See also

[Graphical configuration](graphical_configuration_korean_fsc.md)

[Adapter initialization](adapter_initialization_korean_fsc.md)
