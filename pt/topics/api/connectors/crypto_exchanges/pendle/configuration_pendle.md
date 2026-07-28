# Configuração do conector: Pendle

Configure as propriedades a seguir antes de se conectar à Pendle. A lista foi verificada com [PendleMessageAdapter](xref:StockSharp.Pendle.PendleMessageAdapter).

## Configurações básicas

O editor de conexões mostra primeiro estas configurações.

- `Chain` (`PendleChains`)
- `WalletAddress` (`string`)
- `PrivateKey` (`SecureString`)
- `ApiEndpoint` (`string`)
- `RpcEndpoint` (`string`)

## Configurações avançadas

Estas propriedades controlam a seleção de mercados, os limites, as consultas periódicas e o comportamento das transações.

- `MarketAddresses` (`string`)
- `MaxMarkets` (`int`)
- `ProbeVolume` (`decimal`)
- `SlippageTolerance` (`decimal`)
- `PollingInterval` (`TimeSpan`)
- `HistoryLimit` (`int`)
- `ReceiptTimeout` (`TimeSpan`)
- `IsAutoApprove` (`bool`)

## Veja também

[Configuração gráfica](graphical_configuration_pendle.md)

[Inicialização do adaptador](adapter_initialization_pendle.md)
