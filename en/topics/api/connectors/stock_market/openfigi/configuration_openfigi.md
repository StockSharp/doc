# Connector configuration: OpenFIGI

Configure the following properties before connecting to OpenFIGI. The list is verified against [OpenFigiMessageAdapter](xref:StockSharp.OpenFigi.OpenFigiMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)

## Advanced settings

These properties control provider endpoints, request pacing, filters, data options, and result limits.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)
- `MaximumResults` (`int`)
- `ExchangeCode` (`string`)
- `MicCode` (`string`)
- `Currency` (`string`)
- `MarketSector` (`string`)
- `SecurityType2` (`string`)
- `IncludeUnlistedEquities` (`bool`)

## See also

[Graphical configuration](graphical_configuration_openfigi.md)

[Adapter initialization](adapter_initialization_openfigi.md)
