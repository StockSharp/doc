# Configuração do conector: XRPL DEX

Configure as propriedades a seguir antes de se conectar ao XRPL DEX. A lista foi verificada com [XrplMessageAdapter](xref:StockSharp.Xrpl.XrplMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `RpcEndpoint` (`string`)
- `StreamingEndpoint` (`string`)
- `Account` (`string`)
- `Seed` (`SecureString`)

## Configurações avançadas

Estas propriedades controlam a seleção de mercados, a profundidade do livro, o histórico, as taxas, as consultas periódicas e a proteção de transações.

- `Markets` (`string`)
- `DomainId` (`string`)
- `OrderBookDepth` (`int`)
- `HistoryLedgerLimit` (`int`)
- `FeeMultiplier` (`decimal`)
- `LastLedgerOffset` (`int`)
- `MarketOrderProtection` (`decimal`)
- `PollingInterval` (`TimeSpan`)

## Veja também

[Configuração gráfica](graphical_configuration_xrpl.md)

[Inicialização do adaptador](adapter_initialization_xrpl.md)
