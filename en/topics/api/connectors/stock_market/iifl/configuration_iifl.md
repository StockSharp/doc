# Connector configuration: IIFL

Configure the following properties before connecting to IIFL. The list is verified against [IIFLMessageAdapter](xref:StockSharp.IIFL.IIFLMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)

## Advanced settings

These properties control authentication and session state, provider endpoints, streaming, and polling.

- `AuthorizationCode` (`string`)
- `SessionToken` (`SecureString`)
- `PortfolioName` (`string`)
- `RestEndpoint` (`string`)
- `BridgeHost` (`string`)
- `BridgePort` (`int`)
- `TokenValidationEndpoint` (`string`)
- `StreamingEnabled` (`bool`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_iifl.md)

[Adapter initialization](adapter_initialization_iifl.md)
