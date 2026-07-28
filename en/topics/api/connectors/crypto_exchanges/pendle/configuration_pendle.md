# Connector configuration: Pendle

Configure the following properties before connecting to Pendle. The list is verified against [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Advanced settings

These properties control market selection, limits, polling, and transaction behavior.

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## See also

[Graphical configuration](graphical_configuration_pendle.md)

[Adapter initialization](adapter_initialization_pendle.md)
