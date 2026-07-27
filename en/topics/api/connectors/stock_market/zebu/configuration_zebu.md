# Connector configuration: Zebu

Configure the following properties before connecting to Zebu. The list is verified against [ZebuMessageAdapter](xref:StockSharp.Zebu.ZebuMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `UserId` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `RefreshToken` (`SecureString`)
- `Token` (`SecureString`)
- `AccountId` (`string`)
- `TokenExpiresAt` (`DateTime?`)
- `DefaultProduct` (`ShoonyaProducts`)
- `ReconnectAttempts` (`int`)
- `AuthorizationAddress` (`Uri`)
- `RestEndpoint` (`string`)
- `InstrumentEndpointTemplate` (`string`)
- `WebSocketEndpoint` (`string`)

## See also

[Graphical configuration](graphical_configuration_zebu.md)

[Adapter initialization](adapter_initialization_zebu.md)
