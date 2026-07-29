# Connector configuration: J-Quants

Configure the following properties before connecting to J-Quants. The list is verified against [JQuantsMessageAdapter](xref:StockSharp.JQuants.JQuantsMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)

## Advanced settings

These properties control provider endpoints, request pacing, filters, data options, and result limits.

- `RestEndpoint` (`string`)
- `RequestInterval` (`TimeSpan`)
- `MaximumPages` (`int`)

## See also

[Graphical configuration](graphical_configuration_jquants.md)

[Adapter initialization](adapter_initialization_jquants.md)
