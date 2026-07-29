# Connector configuration: SEC EDGAR

Configure the following properties before connecting to SEC EDGAR. The list is verified against [SecEdgarMessageAdapter](xref:StockSharp.SecEdgar.SecEdgarMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `DataEndpoint` (`Uri`)
- `UserAgent` (`string`)

## Advanced settings

These properties control provider endpoints, request pacing, filters, data options, and result limits.

- `WebsiteEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Forms` (`string`)
- `MaximumHistoricalFiles` (`int`)
- `MaximumFacts` (`int`)

## See also

[Graphical configuration](graphical_configuration_sec_edgar.md)

[Adapter initialization](adapter_initialization_sec_edgar.md)
