# Connector configuration: SimFin

Configure the following properties before connecting to SimFin. The list is verified against [SimFinMessageAdapter](xref:StockSharp.SimFin.SimFinMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)

## Advanced settings

These properties control provider endpoints, request pacing, filters, data options, and result limits.

- `RestEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `StatementTypes` (`string`)
- `Period` (`string`)
- `AsReported` (`bool`)
- `IncludeRatios` (`bool`)
- `MaximumRecords` (`int`)

## See also

[Graphical configuration](graphical_configuration_simfin.md)

[Adapter initialization](adapter_initialization_simfin.md)
