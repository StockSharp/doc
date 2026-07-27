# Connector configuration: FINRA

Configure the following properties before connecting to FINRA. The list is verified against [FinraMessageAdapter](xref:StockSharp.Finra.FinraMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `IsDemo` (`bool`)
- `DataSet` (`FinraDataSets`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Token` (`SecureString`)
- `WeeklyTierIdentifier` (`string`)
- `WeeklySummaryTypeCode` (`string`)
- `PageSize` (`int`)
- `MaxRecords` (`int`)
- `DataVersion` (`int`)
- `Address` (`Uri`)
- `AuthAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_finra.md)

[Adapter initialization](adapter_initialization_finra.md)
