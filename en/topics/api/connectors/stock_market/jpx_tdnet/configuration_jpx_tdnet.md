# Connector configuration: JPX TDnet

Configure the following properties before connecting to JPX TDnet. The list is verified against [JpxTdnetMessageAdapter](xref:StockSharp.JpxTdnet.JpxTdnetMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Address` (`Uri`)
- `ViewerAddress` (`Uri`)
- `IndexMode` (`JpxTdnetIndexModes`)
- `DefaultLookupDays` (`int`)
- `MaxDays` (`int`)
- `SecurityLookupDays` (`int`)
- `RequestInterval` (`TimeSpan`)
- `MaxDocumentSizeMb` (`int`)

## See also

[Graphical configuration](graphical_configuration_jpx_tdnet.md)

[Adapter initialization](adapter_initialization_jpx_tdnet.md)
