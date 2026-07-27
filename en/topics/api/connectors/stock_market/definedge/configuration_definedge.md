# Connector configuration: Definedge

Configure the following properties before connecting to Definedge. The list is verified against [DefinedgeMessageAdapter](xref:StockSharp.Definedge.DefinedgeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Key` (`SecureString`)
- `Secret` (`SecureString`)
- `Token` (`SecureString`)
- `WebSocketToken` (`SecureString`)
- `UserId` (`string`)
- `AccountId` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `OneTimePassword` (`SecureString`)
- `DefaultProduct` (`DefinedgeProducts`)
- `AlgoId` (`string`)
- `Address` (`Uri`)
- `LoginAddress` (`Uri`)
- `HistoryAddress` (`Uri`)
- `InstrumentMasterAddress` (`Uri`)
- `WebSocketAddress` (`Uri`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_definedge.md)

[Adapter initialization](adapter_initialization_definedge.md)
