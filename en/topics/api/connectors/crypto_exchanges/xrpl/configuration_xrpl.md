# Connector configuration: XRPL DEX

Configure the following properties before connecting to XRPL DEX. The list is verified against [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## Advanced settings

These properties control market selection, order-book depth, history, fees, polling, and transaction protection.

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_xrpl.md)

[Adapter initialization](adapter_initialization_xrpl.md)
