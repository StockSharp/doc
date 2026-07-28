# 连接器配置：Chainflip

连接 Chainflip 前，请配置以下适配器属性。该列表已根据 [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter) 的实现进行核对。

## 基本设置

连接编辑器会优先显示这些设置。

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## 高级设置

这些属性用于控制目标地址、池筛选、轮询、订单簿深度和交易行为。

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

## 另请参阅

[图形化配置](graphical_configuration_chainflip.md)

[适配器初始化](adapter_initialization_chainflip.md)
