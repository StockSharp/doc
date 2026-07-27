# Connector configuration: Primary

Configure the following properties before connecting to Primary. The list is verified against [PrimaryMessageAdapter](xref:StockSharp.Primary.PrimaryMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Login` (`string`)
- `Password` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Token` (`SecureString`)
- `Proprietary` (`string`)
- `DefaultMarket` (`string`)
- `MarketDataLevel` (`int`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `SandboxWebSocketAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_primary.md)

[Adapter initialization](adapter_initialization_primary.md)
