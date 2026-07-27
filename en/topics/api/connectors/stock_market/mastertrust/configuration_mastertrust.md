# Connector configuration: Mastertrust

Configure the following properties before connecting to Mastertrust. The list is verified against [MastertrustMessageAdapter](xref:StockSharp.Mastertrust.MastertrustMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ClientId` (`string`)
- `OAuthClientId` (`string`)
- `OAuthClientSecret` (`SecureString`)
- `AuthorizationCode` (`SecureString`)
- `RedirectUri` (`Uri`)
- `Token` (`SecureString`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PortfolioName` (`string`)
- `DefaultProduct` (`MastertrustProducts`)
- `ReconnectAttempts` (`int`)
- `Address` (`Uri`)
- `MasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_mastertrust.md)

[Adapter initialization](adapter_initialization_mastertrust.md)
