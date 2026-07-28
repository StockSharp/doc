# Connector configuration: 0x

Configure the following properties before connecting to 0x. The list is verified against [ZeroXMessageAdapter](xref:StockSharp.ZeroX.ZeroXMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ApiKey` (`SecureString`)
- `Chain` (`ZeroXChains`)
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

[Graphical configuration](graphical_configuration_zero_x.md)

[Adapter initialization](adapter_initialization_zero_x.md)
