# Connector configuration: BCS

Configure the following properties before connecting to BCS. The list is verified against [BcsMessageAdapter](xref:StockSharp.Bcs.BcsMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `IsReadOnly` (`bool`)
- `PortfolioName` (`string`)
- `PollingInterval` (`TimeSpan`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## See also

[Graphical configuration](graphical_configuration_bcs.md)

[Adapter initialization](adapter_initialization_bcs.md)
