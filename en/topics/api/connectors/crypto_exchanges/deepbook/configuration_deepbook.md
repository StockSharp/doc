# Connector configuration: DeepBook

Configure the following properties before connecting to DeepBook. The list is verified against [DeepBookMessageAdapter](xref:StockSharp.DeepBook.DeepBookMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `IndexerEndpoint` (`string`)
- `GrpcEndpoint` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `PackageId` (`string`)
- `ClockObjectId` (`string`)
- `Pools` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLimit` (`int`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_deepbook.md)

[Adapter initialization](adapter_initialization_deepbook.md)
