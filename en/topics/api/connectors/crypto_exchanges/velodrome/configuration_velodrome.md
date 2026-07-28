# Connector configuration: Velodrome

Configure the following properties before connecting to Velodrome. The list is verified against [VelodromeMessageAdapter](xref:StockSharp.Velodrome.VelodromeMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `RpcEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Pools` (`string`)
- `HistoryBlockRange` (`int`)
- `HistoryBlockCount` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## See also

[Graphical configuration](graphical_configuration_velodrome.md)

[Adapter initialization](adapter_initialization_velodrome.md)
