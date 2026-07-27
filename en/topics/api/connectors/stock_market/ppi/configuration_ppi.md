# Connector configuration: PPI

Configure the following properties before connecting to PPI. The list is verified against [PpiMessageAdapter](xref:StockSharp.Ppi.PpiMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `AuthorizedClient` (`string`)
- `ClientKey` (`SecureString`)
- `IsDemo` (`bool`)
- `Account` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Token` (`SecureString`)
- `RefreshToken` (`SecureString`)
- `DefaultMarket` (`string`)
- `DefaultInstrumentType` (`string`)
- `DefaultSettlement` (`string`)
- `AccountPollingInterval` (`TimeSpan`)
- `LookupLimit` (`int`)
- `RestAddress` (`Uri`)
- `SandboxRestAddress` (`Uri`)
- `RealtimeAddress` (`Uri`)
- `SandboxRealtimeAddress` (`Uri`)

## See also

[Graphical configuration](graphical_configuration_ppi.md)

[Adapter initialization](adapter_initialization_ppi.md)
