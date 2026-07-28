# Configuração do conector: Chainflip

Configure as propriedades a seguir antes de se conectar à Chainflip. A lista foi verificada com [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os endereços de destino, os filtros de reservas, as consultas periódicas, a profundidade do livro e o comportamento das transações.

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

## Veja também

[Configuração gráfica](graphical_configuration_chainflip.md)

[Inicialização do adaptador](adapter_initialization_chainflip.md)
