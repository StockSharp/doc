# Connector configuration: Finage

Configure the following properties before connecting to Finage. The list is verified against [FinageMessageAdapter](xref:StockSharp.Finage.FinageMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ApiKey` (`SecureString`)

## Advanced settings

These properties control REST and WebSocket access, endpoints, market-data options, symbol filters, and result limits.

- `StreamingToken` (`SecureString`)
- `RestEndpoint` (`Uri`)
- `StreamingEndpoint` (`Uri`)
- `RequestInterval` (`TimeSpan`)
- `Symbols` (`string`)
- `MaximumSecurities` (`int`)

## See also

[Graphical configuration](graphical_configuration_finage.md)

[Adapter initialization](adapter_initialization_finage.md)
