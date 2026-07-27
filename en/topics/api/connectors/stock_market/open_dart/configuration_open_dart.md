# Connector configuration: Open DART

Configure the following properties before connecting to Open DART. The list is verified against [OpenDartMessageAdapter](xref:StockSharp.OpenDart.OpenDartMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `DisclosureAddress` (`Uri`)
- `DisclosureType` (`OpenDartDisclosureTypes`)
- `CorporationClass` (`OpenDartCorporationClasses`)
- `FinalReportsOnly` (`bool`)
- `BusinessYear` (`int?`)
- `ReportType` (`OpenDartReportTypes`)
- `FinancialSearchYears` (`int`)
- `MaxPages` (`int`)

## See also

[Graphical configuration](graphical_configuration_open_dart.md)

[Adapter initialization](adapter_initialization_open_dart.md)
