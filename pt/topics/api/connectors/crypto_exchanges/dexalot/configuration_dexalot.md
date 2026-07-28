# Configuração do conector: Dexalot

Configure as propriedades a seguir antes de se conectar à Dexalot. A lista foi verificada com [DexalotMessageAdapter](xref:StockSharp.Dexalot.DexalotMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `RestEndpoint` (`string`)
- `WebSocketEndpoint` (`string`)
- `RpcEndpoint` (`string`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam os endereços de contratos, os filtros de pares, a profundidade do livro, as consultas periódicas e o comportamento das transações.

- `TradePairsAddress` (`string`)
- `PortfolioAddress` (`string`)
- `Pairs` (`string`)
- `OrderBookDepth` (`int`)
- `PrivatePollingInterval` (`TimeSpan`)
- `ReceiptTimeout` (`TimeSpan`)
- `SelfTradePrevention` (`DexalotSelfTradePrevention`)

## Veja também

[Configuração gráfica](graphical_configuration_dexalot.md)

[Inicialização do adaptador](adapter_initialization_dexalot.md)
