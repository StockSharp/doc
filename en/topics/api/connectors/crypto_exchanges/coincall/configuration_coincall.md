# Connector configuration: Coincall

Configure the following properties before connecting to Coincall. The list is verified against [CoincallMessageAdapter](xref:StockSharp.Coincall.CoincallMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ProductType` (`CoincallProductTypes`)
- `RestEndpoint` (`string`)
- `OptionsWebSocketEndpoint` (`string`)
- `FuturesWebSocketEndpoint` (`string`)
- `RequestValidityWindow` (`TimeSpan`)
- `PrivatePollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_coincall.md)

[Adapter initialization](adapter_initialization_coincall.md)
