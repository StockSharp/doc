# Connector configuration: KyberSwap

Configure the following properties before connecting to KyberSwap. The list is verified against [KyberSwapMessageAdapter](xref:StockSharp.KyberSwap.KyberSwapMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `ClientId` (`string`)
- `Chain` (`KyberSwapChains`)
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
- `TransactionLifetime` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## See also

[Graphical configuration](graphical_configuration_kyber_swap.md)

[Adapter initialization](adapter_initialization_kyber_swap.md)
