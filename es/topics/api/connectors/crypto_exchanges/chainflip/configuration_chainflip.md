# Configuración del conector: Chainflip

Configure las siguientes propiedades antes de conectarse a Chainflip. La lista se ha verificado con [ChainflipMessageAdapter](xref:StockSharp.Chainflip.ChainflipMessageAdapter).

## Configuración básica

El editor de conexiones muestra primero estos parámetros.

- `StateRpcEndpoint` (`string`)
- `BackendEndpoint` (`string`)
- `EthereumRpcEndpoint` (`string`)
- `ArbitrumRpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Configuración avanzada

Estas propiedades controlan las direcciones de destino, los filtros de fondos, las consultas periódicas, la profundidad del libro y el comportamiento de las transacciones.

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

## Véase también

[Configuración gráfica](graphical_configuration_chainflip.md)

[Inicialización del adaptador](adapter_initialization_chainflip.md)
