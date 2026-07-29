# Connector configuration: SSI

Configure the following properties before connecting to SSI. The list is verified against [SSIMessageAdapter](xref:StockSharp.SSI.SSIMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `ClientId` (`string`)
- `Account` (`string`)

## Advanced settings

These properties control authentication and session state, provider endpoints, streaming, and polling.

- `PrivateKey` (`SecureString`)
- `Otp` (`SecureString`)
- `RestEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_ssi.md)

[Adapter initialization](adapter_initialization_ssi.md)
