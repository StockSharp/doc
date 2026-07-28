# Connector configuration: Birdeye

Configure the following properties before connecting to Birdeye. The list is verified against [BirdeyeMessageAdapter](xref:StockSharp.Birdeye.BirdeyeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Token` (`SecureString`)
- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `WebSocketOrigin` (`string`)
- `Chain` (`string`)
- `TokenAddress` (`string`)
- `StreamingEnabled` (`bool`)
- `PriceInUsd` (`bool`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `MinimumLiquidity` (`decimal`)
- `RequestInterval` (`TimeSpan`)
- `PollingInterval` (`TimeSpan`)
- `MaximumItems` (`int`)
- `HistoryLimit` (`int`)

## See also

[Graphical configuration](graphical_configuration_birdeye.md)

[Adapter initialization](adapter_initialization_birdeye.md)
