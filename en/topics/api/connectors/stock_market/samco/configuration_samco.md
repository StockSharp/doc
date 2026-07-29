# Connector configuration: Samco

Configure the following properties before connecting to Samco. The list is verified against [SamcoMessageAdapter](xref:StockSharp.Samco.SamcoMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)

## Advanced settings

These properties control authentication and session state, provider endpoints, streaming, and polling.

- `Secret` (`SecureString`)
- `SessionToken` (`SecureString`)
- `RestEndpoint` (`string`)
- `InstrumentEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_samco.md)

[Adapter initialization](adapter_initialization_samco.md)
