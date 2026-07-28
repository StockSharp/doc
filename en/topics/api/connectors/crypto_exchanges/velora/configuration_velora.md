# Connector configuration: Velora

Configure the following properties before connecting to Velora. The list is verified against [VeloraMessageAdapter](xref:StockSharp.Velora.VeloraMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `Partner` (`string`)
- `Chain` (`VeloraChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Advanced settings

These properties control endpoints, filters, limits, and other provider-specific behavior.

- `Markets` (`string`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## See also

[Graphical configuration](graphical_configuration_velora.md)

[Adapter initialization](adapter_initialization_velora.md)
