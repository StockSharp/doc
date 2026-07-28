# Connector configuration: Chainflip

Configure the following properties before connecting to Chainflip. The list is verified against [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter).

## Basic settings

The connection editor shows these settings first.

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Advanced settings

These properties control destination addresses, pool filters, polling, order-book depth, and transaction behavior.

- `BitcoinAddress` (`string`)
- `SolanaAddress` (`string`)
- `AssethubAddress` (`string`)
- `PolkadotAddress` (`string`)
- `TronAddress` (`string`)
- `Pools` (`string`)
- `ProbeVolume` (`decimal`)
- `OrderBookDepth` (`int`)
- `PollingInterval` (`TimeSpan`)
- `MaxBlocksPerPoll` (`int`)
- `InitialTickBlocks` (`int`)
- `SlippageTolerance` (`decimal`)
- `RetryDurationBlocks` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## See also

[Graphical configuration](graphical_configuration_chainflip.md)

[Adapter initialization](adapter_initialization_chainflip.md)
