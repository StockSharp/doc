# Connector configuration: EDINET

Configure the following properties before connecting to EDINET. The list is verified against [EdinetMessageAdapter](xref:StockSharp.Edinet.EdinetMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `CodeListAddress` (`Uri`)
- `ViewerAddress` (`Uri`)
- `DisclosureType` (`EdinetDisclosureTypes`)
- `ListedOnly` (`bool`)
- `IncludeWithdrawn` (`bool`)
- `IncludeUnavailable` (`bool`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)
- `CodeListCacheTimeout` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_edinet.md)

[Adapter initialization](adapter_initialization_edinet.md)
